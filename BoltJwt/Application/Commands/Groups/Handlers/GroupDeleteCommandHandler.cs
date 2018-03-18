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
            var group = await _groupRepository.GetAsync(command.Id);

            _groupRepository.CheckUsage(group);

            _groupRepository.Delete(group);

            return await _groupRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}