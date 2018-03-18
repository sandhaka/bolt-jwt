using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Domain.Model.Abstractions;
using MediatR;

namespace BoltJwt.Application.Commands.Users.Handlers
{
    public class EditUserGroupsCommandHandler: IRequestHandler<EditUserGroupsCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IGroupRepository _groupRepository;

        public EditUserGroupsCommandHandler(IUserRepository userRepository, IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<bool> Handle(EditUserGroupsCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetWithGroupsAsync(command.UserId);

            var groups = _groupRepository.Qyery(g => command.Groups.Contains(g.Id));

            user.EditGroups(command.Groups, groups.ToList());

            return await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}