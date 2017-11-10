using BoltJwt.Domain.Model.Abstractions;

namespace BoltJwt.Domain.Model
{
    public class RoleAuthorization : Entity
    {
        public string AuthorizationName { get; set; }

        public int RoleId { get; set; }

        public int DefAuthorizationId { get; set; }
    }
}