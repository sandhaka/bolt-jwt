using System;
using System.Threading.Tasks;
using BoltJwt.Domain.Model.Abstractions;
using MediatR;

namespace BoltJwt.Application.Commands.Authorizations.Handlers
{
    public class AuthorizationDeleteCommandHandler : IAsyncRequestHandler<AuthorizationDeleteCommand, bool>
    {
        private readonly IAuthorizationRepository _authorizationRepository;

        public AuthorizationDeleteCommandHandler(IAuthorizationRepository authorizationRepository)
        {
            _authorizationRepository = authorizationRepository ??
                                       throw new ArgumentNullException(nameof(authorizationRepository));
        }

        public async Task<bool> Handle(AuthorizationDeleteCommand authorizationDeleteCommand)
        {
            await _authorizationRepository.DeleteAsync(authorizationDeleteCommand.Id);

            return await _authorizationRepository.UnitOfWork.SaveEntitiesAsync();
        }
    }
}