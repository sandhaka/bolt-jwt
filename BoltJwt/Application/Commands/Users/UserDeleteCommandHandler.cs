using System.Threading.Tasks;
using BoltJwt.Domain.Model.Abstractions;
using MediatR;

namespace BoltJwt.Application.Commands.Users
{
    public class UserDeleteCommandHandler : IAsyncRequestHandler<UserDeleteCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public UserDeleteCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(UserDeleteCommand userDeleteCommand)
        {
            await _userRepository.DeleteAsync(userDeleteCommand.Id);

            return await _userRepository.UnitOfWork.SaveEntitiesAsync();
        }
    }
}