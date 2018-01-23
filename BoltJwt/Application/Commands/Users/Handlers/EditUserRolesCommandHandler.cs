using System;
using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Domain.Model.Abstractions;
using MediatR;

namespace BoltJwt.Application.Commands.Users.Handlers
{
    public class EditUserRolesCommandHandler : IRequestHandler<EditUserRolesCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public EditUserRolesCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<bool> Handle(EditUserRolesCommand editUserRolesCommand, CancellationToken cancellationToken)
        {
            await _userRepository.EditRolesAsync(editUserRolesCommand.UserId, editUserRolesCommand.Roles);

            return await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}