using System;
using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Domain.Model;
using BoltJwt.Domain.Model.Abstractions;
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

            _roleRepository.Add(role);

            return await _roleRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}