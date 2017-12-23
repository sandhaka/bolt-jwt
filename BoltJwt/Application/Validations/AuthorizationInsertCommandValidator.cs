using BoltJwt.Application.Commands.Authorizations;
using FluentValidation;

namespace BoltJwt.Application.Validations
{
    public class AuthorizationInsertCommandValidator : AbstractValidator<AuthorizationInsertCommand>
    {
        public AuthorizationInsertCommandValidator()
        {
            RuleFor(command => command.Name)
                .NotEmpty()
                .Matches("[a-zA-Z]{3,12}");
        }
    }
}