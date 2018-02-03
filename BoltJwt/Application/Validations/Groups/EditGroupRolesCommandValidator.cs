using BoltJwt.Application.Commands.Groups;
using FluentValidation;

namespace BoltJwt.Application.Validations.Groups
{
    public class EditGroupRolesCommandValidator: AbstractValidator<EditGroupRolesCommand>
    {
        public EditGroupRolesCommandValidator()
        {
            RuleFor(command => command.GroupId).NotNull();
            RuleFor(command => command.Roles).NotNull();
        }
    }
}