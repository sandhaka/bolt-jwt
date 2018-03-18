using System;
using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Domain.Model.Abstractions;
using MediatR;

namespace BoltJwt.Application.Commands.Groups.Handlers
{
    public class GroupEditCommandHandler : IRequestHandler<GroupEditCommand, bool>
    {
        private readonly IGroupRepository _groupRepository;

        public GroupEditCommandHandler(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
        }

        public async Task<bool> Handle(GroupEditCommand command, CancellationToken cancellationToken)
        {
            var group = await _groupRepository.GetAsync(command.Id);

            group.Description = command.Description;

            return await _groupRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}