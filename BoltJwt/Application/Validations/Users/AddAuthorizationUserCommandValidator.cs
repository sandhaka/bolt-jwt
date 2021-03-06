﻿using BoltJwt.Application.Commands.Users;
using FluentValidation;

namespace BoltJwt.Application.Validations.Users
{
    public class AddAuthorizationUserCommandValidator : AbstractValidator<AddAuthorizationUserCommand>
    {
        public AddAuthorizationUserCommandValidator()
        {
            RuleFor(command => command.Authorizations).NotNull().NotEmpty();
            RuleFor(command => command.UserId).NotNull();
        }
    }
}