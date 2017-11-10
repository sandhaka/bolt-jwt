using System;
using System.Threading.Tasks;
using BoltJwt.Controllers.Dto;
using BoltJwt.Domain.Model.Abstractions;
using MediatR;

namespace BoltJwt.Application.Commands.Roles
{
    public class RoleEditCommandHandler : IAsyncRequestHandler<RoleEditCommand, bool>
    {
        private readonly IRoleRepository _roleRepository;

        public RoleEditCommandHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        }

        public async Task<bool> Handle(RoleEditCommand roleEditCommand)
        {
            var roleEditDto = new RoleEditDto
            {
                Id = roleEditCommand.Id,
                Description = roleEditCommand.Description
            };

            await _roleRepository.UpdateAsync(roleEditDto);

            return await _roleRepository.UnitOfWork.SaveEntitiesAsync();
        }
    }
}