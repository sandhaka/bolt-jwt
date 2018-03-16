using System;
using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Domain.Model.Abstractions;
using MediatR;

namespace BoltJwt.Application.Commands.Users.Handlers
{
    public class AddAuthorizationUserCommandHandler : IRequestHandler<AddAuthorizationUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public AddAuthorizationUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<bool> Handle(AddAuthorizationUserCommand addAuthorizationUserCommand, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetWithAutorizationsAsync(addAuthorizationUserCommand.UserId);

            user.AssignAuthorizations(addAuthorizationUserCommand.Authorizations);

            return await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}