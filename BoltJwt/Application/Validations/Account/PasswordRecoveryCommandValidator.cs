using BoltJwt.Application.Commands.Users;
using FluentValidation;

namespace BoltJwt.Application.Validations.Account
{
    public class PasswordRecoveryCommandValidator : AbstractValidator<PasswordRecoveryCommand>
    {
        public PasswordRecoveryCommandValidator()
        {
            RuleFor(command => command.Email).EmailAddress();
        }
    }
}