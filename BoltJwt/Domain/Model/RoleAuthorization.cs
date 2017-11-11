using BoltJwt.Domain.Model.Abstractions;

namespace BoltJwt.Domain.Model
{
    public class RoleAuthorization : Entity
    {
        public int RoleId { get; set; }

        public int DefAuthorizationId { get; set; }
    }
}