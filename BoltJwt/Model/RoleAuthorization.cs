using BoltJwt.Model.Abstractions;

namespace BoltJwt.Model
{
    public class RoleAuthorization : Entity
    {
        public string AuthorizationName { get; set; }

        public int RoleId { get; set; }
    }
}