using System.Collections.Generic;
using BoltJwt.Domain.Model.Abstractions;
using BoltJwt.Domain.Model.Aggregates.Role;
using BoltJwt.Domain.Model.Aggregates.User;

namespace BoltJwt.Domain.Model.Aggregates.Authorization
{
    public class DefinedAuthorization : AggregateRoot
    {
        protected DefinedAuthorization() {}

        public static DefinedAuthorization Create(string name)
        {
            return new DefinedAuthorization(name);
        }

        private DefinedAuthorization(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public IEnumerable<RoleAuthorization> RolesAuthorizations { get; set; }
        public IEnumerable<UserAuthorization> UserAuthorizations { get; set; }
    }
}