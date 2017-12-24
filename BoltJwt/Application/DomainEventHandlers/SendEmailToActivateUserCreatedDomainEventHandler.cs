using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Domain.Events;
using MailKit.Net.Smtp;
using MailKit.Security;
using MediatR;
using MimeKit;

namespace BoltJwt.Application.DomainEventHandlers
{
    public class SendEmailToActivateUserCreatedDomainEventHandler : INotificationHandler<UserCreatedDomainEvent>
    {
        public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var message = new MimeMessage
            {
                From = {new MailboxAddress("", "")},
                To = {new MailboxAddress("", "")},
                Subject = "",
                Body = new TextPart("plain")
                {
                    Text = @""
                }
            };

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