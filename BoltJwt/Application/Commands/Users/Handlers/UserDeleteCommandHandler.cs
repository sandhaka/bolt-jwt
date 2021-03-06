﻿using System;
using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Domain.Model.Abstractions;
using MediatR;

namespace BoltJwt.Application.Commands.Users.Handlers
{
    public class UserDeleteCommandHandler : IRequestHandler<UserDeleteCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public UserDeleteCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<bool> Handle(UserDeleteCommand userDeleteCommand, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(userDeleteCommand.Id);

            _userRepository.Delete(user);

            return await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}