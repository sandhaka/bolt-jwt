using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace BoltJwt.Application.DomainEventHandlers
{
    public class SendEmailToActivateUserCreatedDomainEventHandler : INotificationHandler<UserCreatedDomainEvent>
    {
        private ILogger<SendEmailToActivateUserCreatedDomainEventHandler> _logger;

        public SendEmailToActivateUserCreatedDomainEventHandler(ILogger<SendEmailToActivateUserCreatedDomainEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var message = new MimeMessage
            {
                From = {new MailboxAddress("pippo", "piipo@pippo.it")},
                To = {new MailboxAddress("", "")},
                Subject = "",
                Body = new TextPart("plain")
                {
                    Text = @""
                }
            };

            _logger.LogInformation($"Sending an email to {message.To.Mailboxes.First().Address}");

            // TODO
        }
    }
}