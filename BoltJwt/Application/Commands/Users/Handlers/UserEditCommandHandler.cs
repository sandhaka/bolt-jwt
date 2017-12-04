using System;
using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Application.Commands.Users.Responses;
using BoltJwt.Controllers.Dto;
using BoltJwt.Domain.Model.Abstractions;
using MediatR;

namespace BoltJwt.Application.Commands.Users.Handlers
{
    public class UserEditCommandHandler : IRequestHandler<UserEditCommand, UserEditResponse>
    {
        private readonly IUserRepository _userRepository;

        public UserEditCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<UserEditResponse> Handle(UserEditCommand userEditCommand, CancellationToken cancellationToken)
        {
            var userEditDto = new UserEditDto
            {
                Id = userEditCommand.Id,
                Name = userEditCommand.Name,
                Surname = userEditCommand.Surname,
                UserName = userEditCommand.UserName
            };

            var entity = await _userRepository.UpdateAsync(userEditDto);

            await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return new UserEditResponse
            {
                Command = userEditCommand,
                Id = entity.Id
            };
        }
    }
}