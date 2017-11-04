using System.Threading.Tasks;
using BoltJwt.Controllers.Dto;
using BoltJwt.Domain.Model.Abstractions;
using MediatR;

namespace BoltJwt.Application.Commands.Users
{
    public class UserEditCommandHandler : IAsyncRequestHandler<UserEditCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public UserEditCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(UserEditCommand userEditCommand)
        {
            await _userRepository.UserNameExistsAsync(userEditCommand.UserName);

            var userEditDto = new UserEditDto
            {
                Name = userEditCommand.Name,
                Surname = userEditCommand.Surname,
                UserName = userEditCommand.UserName
            };

            await _userRepository.UpdateAsync(userEditCommand.Id, userEditDto);

            return await _userRepository.UnitOfWork.SaveEntitiesAsync();
        }
    }
}