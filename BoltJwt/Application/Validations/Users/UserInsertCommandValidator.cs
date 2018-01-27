using BoltJwt.Application.Commands.Users;
using FluentValidation;

namespace BoltJwt.Application.Validations.Users
{
    public class UserInsertCommandValidator : AbstractValidator<UserInsertCommand>
    {
        public UserInsertCommandValidator()
        {
            RuleFor(command => command.Email).EmailAddress();
            RuleFor(command => command.Name).NotEmpty();
            RuleFor(command => command.UserName).NotEmpty();
        }
    }
}