using System;
using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Domain.Model.Abstractions;
using MediatR;

namespace BoltJwt.Application.Commands.Groups.Handlers
{
    public class EditGroupRolesCommandHandler: IRequestHandler<EditGroupRolesCommand, bool>
    {
        private readonly IGroupRepository _groupRepository;

        public EditGroupRolesCommandHandler(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
        }

        public async Task<bool> Handle(EditGroupRolesCommand command, CancellationToken cancellationToken)
        {
            await _groupRepository.EditRolesAsync(command.GroupId, command.Roles);

            return await _groupRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}