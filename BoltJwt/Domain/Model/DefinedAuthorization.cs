using System.Collections.Generic;
using BoltJwt.Domain.Model.Abstractions;

namespace BoltJwt.Domain.Model
{
    public class DefinedAuthorization : Entity
    {
        public DefinedAuthorization(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public IEnumerable<RoleAuthorization> RolesAuthorizations { get; set; }
        public IEnumerable<UserAuthorization> UserAuthorizations { get; set; }
    }
}