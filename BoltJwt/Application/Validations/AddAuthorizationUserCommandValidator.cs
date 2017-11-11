using BoltJwt.Application.Commands.Users;
using FluentValidation;

namespace BoltJwt.Application.Validations
{
    public class AddAuthorizationUserCommandValidator : AbstractValidator<AddAuthorizationUserCommand>
    {
        public AddAuthorizationUserCommandValidator()
        {
            RuleFor(command => command.AuthorizationName).NotEmpty();
            RuleFor(command => command.UserId).NotNull();
        }
    }
}