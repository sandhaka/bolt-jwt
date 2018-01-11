using MediatR;

namespace BoltJwt.Domain.Events
{
    public class RootPasswordChangedDomainEvent : INotification
    {
        public string Password { get; set; }
    }
}