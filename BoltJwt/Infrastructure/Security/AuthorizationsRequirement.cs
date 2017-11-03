using Microsoft.AspNetCore.Authorization;

namespace BoltJwt.Infrastructure.Security
{
    public class AuthorizationsRequirement : IAuthorizationRequirement
    {
        public readonly string[] Authorizations;

        public AuthorizationsRequirement(params string[] authorizations)
        {
            Authorizations = authorizations;
        }
    }
}