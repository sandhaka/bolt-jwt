using System;
using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Domain.Model.Abstractions;
using MediatR;

namespace BoltJwt.Application.Commands.Groups.Handlers
{
    public class GroupDeleteCommandHandler : IRequestHandler<GroupDeleteCommand, bool>
    {
        private readonly IGroupRepository _groupRepository;

        public GroupDeleteCommandHandler(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
        }

        public async Task<bool> Handle(GroupDeleteCommand command, CancellationToken cancellationToken)
        {
            await _groupRepository.DeleteAsync(command.Id);

            return await _groupRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}