using System;
using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Controllers.Dto;
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
            var userEditDto = new UserEditDto
            {
                Id = userEditCommand.Id,
                Name = userEditCommand.Name,
                Surname = userEditCommand.Surname,
                UserName = userEditCommand.UserName
            };

            await _userRepository.UpdateAsync(userEditDto);

            return await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}