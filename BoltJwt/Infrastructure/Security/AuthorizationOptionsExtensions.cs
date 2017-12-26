using BoltJwt.Domain.Model;
using Microsoft.AspNetCore.Authorization;

namespace BoltJwt.Infrastructure.Security
{
    public static class AuthorizationOptionsExtensions
    {
        /// <summary>
        /// Add here the application policies
        /// </summary>
        /// <param name="authOptions"></param>
        /// <returns></returns>
        public static AuthorizationOptions AddCustomPolicies(this AuthorizationOptions authOptions)
        {
            // Authorize admin users
            authOptions.AddPolicy("bJwtAdmins", policyBuilder => policyBuilder.AddRequirements(
                new AuthorizationsRequirement(Constants.AdministrativeAuth)));

            // Authorize only root
            authOptions.AddPolicy("bJwtRoot", policyBuilder => policyBuilder.AddRequirements(
                new AuthorizationsRequirement()));

            return authOptions;
        }
    }
}