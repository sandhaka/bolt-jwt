using System;
using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Domain.Model;
using BoltJwt.Domain.Model.Abstractions;
using MediatR;

namespace BoltJwt.Application.Commands.Users.Handlers
{
    public class UserInsertCommandHandler : IRequestHandler<UserInsertCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public UserInsertCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(UserInsertCommand userInsertCommand, CancellationToken cancellationToken)
        {
            await _userRepository.UserNameExistsAsync(userInsertCommand.UserName);
            await _userRepository.EmailExistsAsync(userInsertCommand.Email);

            var newUser = new User(
                userInsertCommand.Name,
                userInsertCommand.Surname,
                userInsertCommand.UserName,
                userInsertCommand.Email,

                // Create a code to activate
                new UserActivationCode
                {
                    Code = Guid.NewGuid().ToString(),
                    Timestamp = DateTime.Now.Ticks
                },

                // Insert with a temporary password
                User.PasswordEncrypt(Guid.NewGuid().ToString())
            );

            _userRepository.Add(newUser);

            return await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}