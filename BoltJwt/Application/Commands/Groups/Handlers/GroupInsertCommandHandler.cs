using System;
using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Domain.Model;
using BoltJwt.Domain.Model.Abstractions;
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
            var group = new Group
            {
                Description = command.Description
            };

            _groupRepository.Add(group);

            return await _groupRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}