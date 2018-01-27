using BoltJwt.Application.Commands.Users;
using FluentValidation;

namespace BoltJwt.Application.Validations.Account
{
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(command => command.UserId).NotNull();
            RuleFor(command => command.Code).NotEmpty();
            RuleFor(command => command.Password).NotEmpty();
            RuleFor(command => command.ConfirmPassword).NotEmpty();

            // Check if the password and the password confirmation match
            RuleFor(command => command.Password).Must((command, pwd) => pwd.Equals(command.ConfirmPassword));
        }
    }
}