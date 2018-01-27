using System;
using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Controllers.Dto;
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
            var groupEditDto = new GroupEditDto
            {
                Id = command.Id,
                Description = command.Description
            };

            await _groupRepository.UpdateAsync(groupEditDto);

            return await _groupRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}