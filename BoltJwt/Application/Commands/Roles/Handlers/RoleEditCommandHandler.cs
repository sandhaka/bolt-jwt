using System;
using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Controllers.Dto;
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
            var roleEditDto = new RoleEditDto
            {
                Id = roleEditCommand.Id,
                Description = roleEditCommand.Description
            };

            await _roleRepository.UpdateAsync(roleEditDto);

            return await _roleRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}