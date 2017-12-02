using System;
using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Domain.Model.Abstractions;
using MediatR;

namespace BoltJwt.Application.Commands.Roles.Handlers
{
    public class RoleDeleteCommandHandler : IRequestHandler<RoleDeleteCommand, bool>
    {
        private readonly IRoleRepository _roleRepository;

        public RoleDeleteCommandHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        }

        public async Task<bool> Handle(RoleDeleteCommand roleDeleteCommand, CancellationToken cancellationToken)
        {
            await _roleRepository.DeleteAsync(roleDeleteCommand.Id);

            return await _roleRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}