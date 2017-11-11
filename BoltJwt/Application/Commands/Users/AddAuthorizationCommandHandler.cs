using System;
using System.Threading.Tasks;
using BoltJwt.Domain.Model.Abstractions;
using MediatR;

namespace BoltJwt.Application.Commands.Users
{
    public class AddAuthorizationCommandHandler : IAsyncRequestHandler<AddAuthorizationUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public AddAuthorizationCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<bool> Handle(AddAuthorizationUserCommand addAuthorizationUserCommand)
        {
            await _userRepository.AssignAuthorizationAsync(addAuthorizationUserCommand.UserId,
                addAuthorizationUserCommand.AuthorizationName);

            return await _userRepository.UnitOfWork.SaveEntitiesAsync();
        }
    }
}