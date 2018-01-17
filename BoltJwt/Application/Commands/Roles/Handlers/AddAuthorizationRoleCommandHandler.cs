using System;
using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Domain.Model.Abstractions;
using MediatR;

namespace BoltJwt.Application.Commands.Roles.Handlers
{
    public class AddAuthorizationRoleCommandHandler: IRequestHandler<AddAuthorizationRoleCommand, bool>
    {
        private readonly IRoleRepository _roleRepository;

        public AddAuthorizationRoleCommandHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        }

        public async Task<bool> Handle(AddAuthorizationRoleCommand command, CancellationToken cancellationToken)
        {
            await _roleRepository.AssignAuthorizationsAsync(command.RoleId, command.Authorizations);

            return await _roleRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}