using BoltJwt.Domain.Model.Abstractions;

namespace BoltJwt.Domain.Model
{
    public class UserAuthorization : Entity
    {
        public int UserId { get; set; }

        public int DefAuthorizationId { get; set; }
    }
}