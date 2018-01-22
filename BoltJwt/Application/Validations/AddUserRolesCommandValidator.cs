using BoltJwt.Application.Commands.Users;
using FluentValidation;

namespace BoltJwt.Application.Validations
{
    public class AddUserRolesCommandValidator : AbstractValidator<AddUserRolesCommand>
    {
        public AddUserRolesCommandValidator()
        {
            RuleFor(command => command.UserId).NotNull();
            RuleFor(command => command.Roles).NotNull();
        }
    }
}