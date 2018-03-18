using System;
using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Domain.Model.Abstractions;
using MediatR;

namespace BoltJwt.Application.Commands.Users.Handlers
{
    public class UserEditCommandHandler : IRequestHandler<UserEditCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public UserEditCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<bool> Handle(UserEditCommand userEditCommand, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(userEditCommand.Id);

            _userRepository.CheckForDuplicates(userEditCommand.UserName);

            user.Update(userEditCommand.Name, userEditCommand.Surname, userEditCommand.UserName);

            return await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}