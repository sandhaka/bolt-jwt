using BoltJwt.Application.Commands.Tokens;
using FluentValidation;

namespace BoltJwt.Application.Validations.Tokens
{
    public class ValidateTokenCommandValidator : AbstractValidator<ValidateTokenCommand>
    {
        public ValidateTokenCommandValidator()
        {
            RuleFor(command => command.Token).NotEmpty();
            RuleFor(command => command.Id).NotEmpty();
        }
    }
}