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

        public RoleInsertCommandHandler(IRoleRepository roleRepository, IAuthorizationRepository authorizationRepository)
        {
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