using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Domain.Model.Abstractions;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace BoltJwt.Application.Services
{
    /// <summary>
    /// Wrapper on mailkit smtpclient object
    /// Provides methods to send an email
    /// </summary>
    public class MailService : IMailService
    {
        private readonly IConfigurationRepository _configurationRepository;

        public MailService(IConfigurationRepository configurationRepository)
        {
            _configurationRepository = configurationRepository;
        }

        public async Task SendAnEmailAsync(MimeMessage message, CancellationToken cancellationToken)
        {
            // Retrieve the current configuration
            var config = await _configurationRepository.GetAsync();

            using (var client = new SmtpClient())
            {
                // Accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;

                await client.ConnectAsync(config.SmtpHostName, config.SmtpPort, SecureSocketOptions.Auto, cancellationToken);

                client.AuthenticationMechanisms.Remove("XOAUTH2");

                await client.AuthenticateAsync(config.SmtpUserName, config.SmtpPassword, cancellationToken);

                await client.SendAsync(message, cancellationToken);

                await client.DisconnectAsync(true, cancellationToken);
            }
        }
    }
}