using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Application.Services;
using BoltJwt.Domain.Events;
using BoltJwt.Infrastructure.AppConfigurations;
using MediatR;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace BoltJwt.Application.DomainEventHandlers
{
    /// <summary>
    /// Handle the user created event
    /// Send an email to the new user in order to customize his password and activate it
    /// </summary>
    public class SendEmailToActivateUserCreatedDomainEventHandler : INotificationHandler<UserCreatedDomainEvent>
    {
        private readonly ILogger<SendEmailToActivateUserCreatedDomainEventHandler> _logger;
        private readonly IMailService _mailService;
        private readonly IConfigurationRepository _configurationRepository;

        public SendEmailToActivateUserCreatedDomainEventHandler(
            ILogger<SendEmailToActivateUserCreatedDomainEventHandler> logger,
            IConfigurationRepository configurationRepository,
            IMailService mailService)
        {
            _logger = logger;
            _mailService = mailService;
            _configurationRepository = configurationRepository;
        }

        public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            // TODO: Add to config email text customization
            var config = await _configurationRepository.GetAsync();

            var message = new MimeMessage
            {
                From = {new MailboxAddress("Boltjwt automation", config.SmtpEmail)},
                To = {new MailboxAddress(notification.Name + " " + notification.Surname, notification.Email)},
                Subject = "Activate your account",
                Body = new TextPart("plain")
                {
                    Text = "Account creation confirmation" + Environment.NewLine +
                           "-----------------------------" + Environment.NewLine + Environment.NewLine +
                           "Customize yuor password and activate your account. " + Environment.NewLine +
                           $"http://{config.EndpointFqdn}:{config.EndpointPort}/#/account/activation/{notification.ActivationCode}"
                }
            };

            _logger.LogInformation($"Sending the user activation email to {message.To.Mailboxes.First().Address}");

            await _mailService.SendAnEmailAsync(message, cancellationToken);
        }
    }
}