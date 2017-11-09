using System;
using System.Threading.Tasks;
using BoltJwt.Domain.Model;
using BoltJwt.Domain.Model.Abstractions;
using BoltJwt.Infrastructure.Repositories.Exceptions;
using MediatR;

namespace BoltJwt.Application.Commands.Roles
{
    public class RoleInsertCommandHandler : IAsyncRequestHandler<RoleInsertCommand, bool>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IAuthorizationRepository _authorizationRepository;

        public RoleInsertCommandHandler(IRoleRepository roleRepository, IAuthorizationRepository authorizationRepository)
        {
            _authorizationRepository = authorizationRepository ?? throw new ArgumentNullException(nameof(authorizationRepository));
            _roleRepository = roleRepository  ?? throw new ArgumentNullException(nameof(roleRepository));
        }

        public async Task<bool> Handle(RoleInsertCommand roleInsertCommand)
        {
            var role = new Role
            {
                Description = roleInsertCommand.Description
            };

            foreach (var authorization in roleInsertCommand.Authorizations)
            {
                // Check if the authorization is valid (check if a definition of it exists)
                if (!_authorizationRepository.ContainsAuthorization(authorization))
                {
                    throw new UnknowAuthorization(authorization);
                }

                role.AddAuthorization(authorization);
            }

            _roleRepository.Add(role);

            return await _roleRepository.UnitOfWork.SaveEntitiesAsync();
        }
    }
}