﻿using System;
using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Domain.Model.Abstractions;
using MediatR;

namespace BoltJwt.Application.Commands.Authorizations.Handlers
{
    public class AuthorizationDeleteCommandHandler : IRequestHandler<AuthorizationDeleteCommand, bool>
    {
        private readonly IAuthorizationRepository _authorizationRepository;

        public AuthorizationDeleteCommandHandler(IAuthorizationRepository authorizationRepository)
        {
            _authorizationRepository = authorizationRepository ??
                                       throw new ArgumentNullException(nameof(authorizationRepository));
        }

        public async Task<bool> Handle(AuthorizationDeleteCommand authorizationDeleteCommand, CancellationToken cancellationToken)
        {
            var authorization = await _authorizationRepository.GetAsync(authorizationDeleteCommand.Id);

            _authorizationRepository.Delete(authorization);

            return await _authorizationRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}