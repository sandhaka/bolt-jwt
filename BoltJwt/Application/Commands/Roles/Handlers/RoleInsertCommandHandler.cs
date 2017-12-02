using System;
using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Domain.Model;
using BoltJwt.Domain.Model.Abstractions;
using BoltJwt.Infrastructure.Repositories.Exceptions;
using MediatR;

namespace BoltJwt.Application.Commands.Roles.Handlers
{
    public class RoleInsertCommandHandler : IRequestHandler<RoleInsertCommand, bool>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IAuthorizationRepository _authorizationRepository;

        public RoleInsertCommandHandler(IRoleRepository roleRepository, IAuthorizationRepository authorizationRepository)
        {
            _authorizationRepository = authorizationRepository ?? throw new ArgumentNullException(nameof(authorizationRepository));
            _roleRepository = roleRepository  ?? throw new ArgumentNullException(nameof(roleRepository));
        }

        public async Task<bool> Handle(RoleInsertCommand roleInsertCommand, CancellationToken cancellationToken)
        {
            var role = new Role
            {
                Description = roleInsertCommand.Description
            };

            foreach (var authorization in roleInsertCommand.Authorizations)
            {
                // Check if the authorization is valid (check if a definition of it exists)
                var definedAuthorization = await _authorizationRepository.GetByNameAsync(authorization) ??
                                           throw new EntityNotFoundException(nameof(DefinedAuthorization));

                role.AddAuthorization(definedAuthorization);
            }

            _roleRepository.Add(role);

            return await _roleRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}