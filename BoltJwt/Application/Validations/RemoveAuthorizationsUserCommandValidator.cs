using BoltJwt.Application.Commands.Users;
using FluentValidation;

namespace BoltJwt.Application.Validations
{
    public class RemoveAuthorizationsUserCommandValidator : AbstractValidator<RemoveAuthorizationUserCommand>
    {
        public RemoveAuthorizationsUserCommandValidator()
        {
            RuleFor(command => command.Authorizations).NotNull();
            RuleFor(command => command.UserId).NotNull();
        }
    }
}