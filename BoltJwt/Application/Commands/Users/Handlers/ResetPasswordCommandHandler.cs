using System;
using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Domain.Model.Abstractions;
using BoltJwt.Infrastructure.Extensions;
using MediatR;

namespace BoltJwt.Application.Commands.Users.Handlers
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public ResetPasswordCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<bool> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(command.UserId);

            user.EditPassword(command.Code, command.Password.ToMd5Hash());

            _userRepository.Update(user);

            return await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}