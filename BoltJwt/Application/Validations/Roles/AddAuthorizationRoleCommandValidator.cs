using BoltJwt.Application.Commands.Roles;
using FluentValidation;

namespace BoltJwt.Application.Validations.Roles
{
    public class AddAuthorizationRoleCommandValidator: AbstractValidator<AddAuthorizationRoleCommand>
    {
        public AddAuthorizationRoleCommandValidator()
        {
            RuleFor(command => command.Authorizations).NotNull().NotEmpty();
            RuleFor(command => command.RoleId).NotNull();
        }
    }
}