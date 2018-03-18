using System;
using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Domain.Model.Abstractions;
using MediatR;

namespace BoltJwt.Application.Commands.Roles.Handlers
{
    public class RoleEditCommandHandler : IRequestHandler<RoleEditCommand, bool>
    {
        private readonly IRoleRepository _roleRepository;

        public RoleEditCommandHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        }

        public async Task<bool> Handle(RoleEditCommand roleEditCommand, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetAsync(roleEditCommand.Id);

            role.Description = roleEditCommand.Description;

            return await _roleRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}