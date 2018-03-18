using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Domain.Model.Abstractions;
using MediatR;

namespace BoltJwt.Application.Commands.Roles.Handlers
{
    public class AddAuthorizationRoleCommandHandler: IRequestHandler<AddAuthorizationRoleCommand, bool>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IAuthorizationRepository _authorizationRepository;

        public AddAuthorizationRoleCommandHandler(IRoleRepository roleRepository,
            IAuthorizationRepository authorizationRepository)
        {
            _authorizationRepository = authorizationRepository ??
                                       throw new ArgumentNullException(nameof(authorizationRepository));
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        }

        public async Task<bool> Handle(AddAuthorizationRoleCommand command, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetWithAuthorizationsAsync(command.RoleId);

            var authorizations = _authorizationRepository.Query(a => command.Authorizations.Contains(a.Id));

            role.AddAuthorizations(command.Authorizations, authorizations.ToList());

            return await _roleRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}