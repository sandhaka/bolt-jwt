using BoltJwt.Application.Commands.Users;
using FluentValidation;

namespace BoltJwt.Application.Validations
{
    public class UserInsertCommandValidator : AbstractValidator<UserInsertCommand>
    {
        public UserInsertCommandValidator()
        {
            RuleFor(command => command.Email).EmailAddress();
            RuleFor(command => command.Name).NotEmpty();
            RuleFor(command => command.UserName).NotEmpty();
            RuleFor(command => command.Password).MinimumLength(6);
        }
    }
}