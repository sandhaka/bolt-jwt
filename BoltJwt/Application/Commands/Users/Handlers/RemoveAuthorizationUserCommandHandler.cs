using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Domain.Model.Abstractions;
using MediatR;

namespace BoltJwt.Application.Commands.Users.Handlers
{
    public class RemoveAuthorizationUserCommandHandler : IRequestHandler<RemoveAuthorizationUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public RemoveAuthorizationUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<bool> Handle(RemoveAuthorizationUserCommand removeAuthorizationUserCommand, CancellationToken cancellationToken)
        {
            await _userRepository.RemoveAuthorizationAsync(removeAuthorizationUserCommand.UserId,
                removeAuthorizationUserCommand.Authorizations.Split(',').Select(int.Parse));

            return await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}