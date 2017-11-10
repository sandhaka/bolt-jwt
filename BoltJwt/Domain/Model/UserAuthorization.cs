using BoltJwt.Domain.Model.Abstractions;

namespace BoltJwt.Domain.Model
{
    public class UserAuthorization : Entity
    {
        public string AuthorizationName { get; set; }

        public int UserId { get; set; }

        public int DefAuthorizationId { get; set; }
    }
}