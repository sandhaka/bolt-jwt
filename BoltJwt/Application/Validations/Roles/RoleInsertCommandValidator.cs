using BoltJwt.Application.Commands.Roles;
using FluentValidation;

namespace BoltJwt.Application.Validations.Roles
{
    public class RoleInsertCommandValidator : AbstractValidator<RoleInsertCommand>
    {
        public RoleInsertCommandValidator()
        {
            RuleFor(command => command.Description).NotEmpty();
        }
    }
}