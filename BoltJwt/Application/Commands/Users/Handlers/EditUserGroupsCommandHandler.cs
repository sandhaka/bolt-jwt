using System;
using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Domain.Model.Abstractions;
using MediatR;

namespace BoltJwt.Application.Commands.Users.Handlers
{
    public class EditUserGroupsCommandHandler: IRequestHandler<EditUserGroupsCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public EditUserGroupsCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<bool> Handle(EditUserGroupsCommand command, CancellationToken cancellationToken)
        {
            await _userRepository.EditGroupsAsync(command.UserId, command.Groups);

            return await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}