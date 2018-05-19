using System;
using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Domain.Model.Abstractions;
using BoltJwt.Domain.Model.Aggregates.Group;
using MediatR;

namespace BoltJwt.Application.Commands.Groups.Handlers
{
    public class GroupInsertCommandHandler : IRequestHandler<GroupInsertCommand, bool>
    {
        private readonly IGroupRepository _groupRepository;

        public GroupInsertCommandHandler(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
        }

        public async Task<bool> Handle(GroupInsertCommand command, CancellationToken cancellationToken)
        {
            var group = Group.Create(command.Description);

            _groupRepository.Add(group);

            return await _groupRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}