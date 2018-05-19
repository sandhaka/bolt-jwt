using BoltJwt.Domain.Model.Abstractions;

namespace BoltJwt.Domain.Model.Aggregates.User
{
    public class UserActivationCode : Entity
    {
        public string Code { get; set; }
        public long Timestamp { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}