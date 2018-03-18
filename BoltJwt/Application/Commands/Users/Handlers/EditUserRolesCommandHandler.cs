using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Domain.Model.Abstractions;
using MediatR;

namespace BoltJwt.Application.Commands.Users.Handlers
{
    public class EditUserRolesCommandHandler : IRequestHandler<EditUserRolesCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public EditUserRolesCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<bool> Handle(EditUserRolesCommand editUserRolesCommand, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetWithRolesAsync(editUserRolesCommand.UserId);

            var roles = _roleRepository.Query(r => editUserRolesCommand.Roles.Contains(r.Id));

            user.EditRoles(editUserRolesCommand.Roles, roles.ToList());

            return await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}