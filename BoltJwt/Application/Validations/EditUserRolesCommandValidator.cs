using BoltJwt.Application.Commands.Users;
using FluentValidation;

namespace BoltJwt.Application.Validations
{
    public class EditUserRolesCommandValidator : AbstractValidator<EditUserRolesCommand>
    {
        public EditUserRolesCommandValidator()
        {
            RuleFor(command => command.UserId).NotNull();
            RuleFor(command => command.Roles).NotNull();
        }
    }
}