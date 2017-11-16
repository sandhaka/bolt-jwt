using System;
using System.Threading.Tasks;
using BoltJwt.Domain.Model;
using BoltJwt.Domain.Model.Abstractions;
using BoltJwt.Infrastructure.Repositories.Exceptions;
using MediatR;

namespace BoltJwt.Application.Commands.Authorizations.Handlers
{
    public class AuthorizationInsertCommandHandler : IAsyncRequestHandler<AuthorizationInsertCommand, bool>
    {
        private readonly IAuthorizationRepository _authorizationRepository;

        public AuthorizationInsertCommandHandler(IAuthorizationRepository authorizationRepository)
        {
            _authorizationRepository = authorizationRepository ??
                                       throw new ArgumentNullException(nameof(authorizationRepository));
        }

        public async Task<bool> Handle(AuthorizationInsertCommand authorizationInsertCommand)
        {
            var auth = new DefinedAuthorization(authorizationInsertCommand.Name);

            if (_authorizationRepository.ContainsAuthorization(authorizationInsertCommand.Name))
            {
                throw new PropertyIndexExistsException(nameof(auth.Name));
            }

            _authorizationRepository.Add(auth);

            return await _authorizationRepository.UnitOfWork.SaveEntitiesAsync();
        }
    }
}