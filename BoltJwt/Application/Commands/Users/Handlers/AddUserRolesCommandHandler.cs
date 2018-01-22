using System;
using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Domain.Model.Abstractions;
using MediatR;

namespace BoltJwt.Application.Commands.Users.Handlers
{
    public class AddUserRolesCommandHandler : IRequestHandler<AddUserRolesCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public AddUserRolesCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<bool> Handle(AddUserRolesCommand addUserRolesCommand, CancellationToken cancellationToken)
        {
            await _userRepository.AssignRoleAsync(addUserRolesCommand.UserId, addUserRolesCommand.Roles);

            return await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}