﻿using System;
using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Domain.Model;
using BoltJwt.Domain.Model.Abstractions;
using MediatR;

namespace BoltJwt.Application.Commands.Authorizations.Handlers
{
    public class AuthorizationInsertCommandHandler : IRequestHandler<AuthorizationInsertCommand, bool>
    {
        private readonly IAuthorizationRepository _authorizationRepository;

        public AuthorizationInsertCommandHandler(IAuthorizationRepository authorizationRepository)
        {
            _authorizationRepository = authorizationRepository ??
                                       throw new ArgumentNullException(nameof(authorizationRepository));
        }

        public async Task<bool> Handle(AuthorizationInsertCommand authorizationInsertCommand, CancellationToken cancellationToken)
        {
            var auth = new DefinedAuthorization(authorizationInsertCommand.Name);

            _authorizationRepository.Add(auth);

            return await _authorizationRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}