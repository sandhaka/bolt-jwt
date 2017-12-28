using System;
using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Domain.Model.Abstractions;
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
            await _userRepository.ActivateUserAsync(command.Code, command.Password);

            return await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}