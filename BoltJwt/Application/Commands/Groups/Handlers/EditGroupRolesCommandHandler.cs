using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Domain.Model.Abstractions;
using MediatR;

namespace BoltJwt.Application.Commands.Groups.Handlers
{
    public class EditGroupRolesCommandHandler: IRequestHandler<EditGroupRolesCommand, bool>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IRoleRepository _roleRepository;

        public EditGroupRolesCommandHandler(IGroupRepository groupRepository, IRoleRepository roleRepository)
        {
            _groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        }

        public async Task<bool> Handle(EditGroupRolesCommand command, CancellationToken cancellationToken)
        {
            var group = await _groupRepository.GetWithRolesAsync(command.GroupId);

            var roles = _roleRepository.Query(r => command.Roles.Contains(r.Id));

            group.EditRoles(command.Roles, roles.ToList());

            return await _groupRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}