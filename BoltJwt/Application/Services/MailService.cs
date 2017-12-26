using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Infrastructure.AppConfigurations;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace BoltJwt.Application.Services
{
    public class MailService : IMailService
    {
        private readonly IConfigurationRepository _configurationRepository;

        public MailService(IConfigurationRepository configurationRepository)
        {
            _configurationRepository = configurationRepository;
        }

        public async Task SendAnEmailAsync(MimeMessage message, CancellationToken cancellationToken)
        {
            using (var client = new SmtpClient())
            {
                // Accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;

                await client.ConnectAsync("", 00, SecureSocketOptions.None, cancellationToken);

                client.AuthenticationMechanisms.Remove("XOAUTH2");

                await client.AuthenticateAsync("", "", cancellationToken);

                await client.SendAsync(message, cancellationToken);

                await client.DisconnectAsync(true, cancellationToken);
            }
        }
    }
}