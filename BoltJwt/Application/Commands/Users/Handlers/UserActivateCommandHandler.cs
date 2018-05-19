using System;
using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Domain.Model.Abstractions;
using BoltJwt.Infrastructure.Extensions;
using MediatR;

namespace BoltJwt.Application.Commands.Users.Handlers
{
    public class UserActivateCommandHandler : IRequestHandler<UserActivateCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public UserActivateCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<bool> Handle(UserActivateCommand command, CancellationToken cancellationToken)
        {
            var userActivationCode = await _userRepository.GetUserActivationCode(command.Code);

            var user = await _userRepository.GetAsync(userActivationCode.UserId);

            user.ActivateUser(command.Password.ToMd5Hash());

            _userRepository.DeleteUserActivationCode(userActivationCode);

            _userRepository.Update(user);

            return await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}