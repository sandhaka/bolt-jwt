using BoltJwt.Application.Commands.Users;
using FluentValidation;

namespace BoltJwt.Application.Validations.Users
{
    public class EditUserGroupsCommandValidator: AbstractValidator<EditUserGroupsCommand>
    {
        public EditUserGroupsCommandValidator()
        {
            RuleFor(command => command.UserId).NotNull();
            RuleFor(command => command.Groups).NotNull();
        }
    }
}