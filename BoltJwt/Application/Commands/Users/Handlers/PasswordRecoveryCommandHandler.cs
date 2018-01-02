using System;
using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Application.Services;
using BoltJwt.Domain.Model.Abstractions;
using BoltJwt.Infrastructure.AppConfigurations;
using MediatR;
using MimeKit;

namespace BoltJwt.Application.Commands.Users.Handlers
{
    public class PasswordRecoveryCommandHandler : IRequestHandler<PasswordRecoveryCommand, bool>
    {
        private readonly IMailService _mailService;
        private readonly IConfigurationRepository _configurationRepository;
        private readonly IUserRepository _userRepository;

        public PasswordRecoveryCommandHandler(
            IMailService mailService,
            IConfigurationRepository configurationRepository,
            IUserRepository userRepository)
        {
            _mailService = mailService;
            _configurationRepository = configurationRepository;
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(PasswordRecoveryCommand command, CancellationToken cancellationToken)
        {
            // Get the user by email
            var user = await _userRepository.GetUserByEmailAsync(command.Email);

            var config = await _configurationRepository.GetAsync();

            // Generate an authorization code
            var authorizationCode = await _userRepository.GenerateForgotPasswordAuthorizationCodeAsync(user.Id);

            // Create the message
            var message = new MimeMessage
            {
                From = {new MailboxAddress("Boltjwt automation - authentication service", config.SmtpEmail)},
                To = {new MailboxAddress($"{user.Name} {user.Surname}", user.Email)},
                Subject = "Password recovery",
                Body = new TextPart("plain")
                {
                    Text = "Click on the following link to reset your password: " +
                           Environment.NewLine +
                           Environment.NewLine +
                           $"http://{config.EndpointFqdn}:{config.EndpointPort}/#/account/password-recovery/{user.Id}/{authorizationCode}"
                }
            };

            await _mailService.SendAnEmailAsync(message, cancellationToken);

            return await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}