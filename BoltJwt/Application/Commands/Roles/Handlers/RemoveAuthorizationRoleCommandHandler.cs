using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Domain.Model.Abstractions;
using MediatR;

namespace BoltJwt.Application.Commands.Roles.Handlers
{
    public class RemoveAuthorizationRoleCommandHandler: IRequestHandler<RemoveAuthorizationRoleCommand, bool>
    {
        private readonly IRoleRepository _roleRepository;

        public RemoveAuthorizationRoleCommandHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        }

        public async Task<bool> Handle(RemoveAuthorizationRoleCommand command, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetWithAuthorizationsAsync(command.RoleId);

            role.RemoveAuthorizations(command.Authorizations.Split(',').Select(int.Parse));

            return await _roleRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}