using System;
using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Domain.Model.Abstractions;
using MediatR;

namespace BoltJwt.Application.Commands.Users.Handlers
{
    public class AddRoleUserCommandHandler : IRequestHandler<AddRoleUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public AddRoleUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<bool> Handle(AddRoleUserCommand addRoleUserCommand, CancellationToken cancellationToken)
        {
            await _userRepository.AssignRoleAsync(addRoleUserCommand.UserId, addRoleUserCommand.RoleId);

            return await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}