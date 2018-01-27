using BoltJwt.Application.Commands.Groups;
using FluentValidation;

namespace BoltJwt.Application.Validations.Groups
{
    public class GroupInsertCommandValidator: AbstractValidator<GroupInsertCommand>
    {
        public GroupInsertCommandValidator()
        {
            RuleFor(command => command.Description).NotEmpty();
        }
    }
}