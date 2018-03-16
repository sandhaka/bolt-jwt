using System;
using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Domain.Events;
using BoltJwt.Domain.Model.Abstractions;
using MediatR;

namespace BoltJwt.Application.DomainEventHandlers
{
    /// <summary>
    /// Handle the root password changed event in sys configuration
    /// Propagate the change to the user entity
    /// </summary>
    public class RootPasswordChangedDomainEventHandler : INotificationHandler<RootPasswordChangedDomainEvent>
    {
        private readonly IUserRepository _userRepository;

        public RootPasswordChangedDomainEventHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task Handle(RootPasswordChangedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            var rootUser = await _userRepository.GetRootAsync();

            rootUser.EditRootPassword(domainEvent.Password);
        }
    }
}