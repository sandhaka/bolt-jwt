using MediatR;

namespace BoltJwt.Domain.Events
{
    public class UserCreatedDomainEvent : INotification
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string ActivationCode { get; set; }
    }
}