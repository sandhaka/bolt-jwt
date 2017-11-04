using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace BoltJwt.Infrastructure.Security
{
    /// <summary>
    /// Authorization handler to check the authorization of the user
    /// </summary>
    public class AuthorizationsHandler : AuthorizationHandler<AuthorizationsRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationsRequirement requirement)
        {
            /*
             * Check if the user is the root
             * Root is a omnipotent user
             */
            var rootClaim = context.User.Claims.FirstOrDefault(i => i.Type == "isRoot")?.Value;

            if (!string.IsNullOrEmpty(rootClaim) && bool.Parse(rootClaim))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            /*
             * Otherwise check the custom authorizations list
             */

            // Get the authorizations list from the claims contained in the token obtained before
            var authorizationsList = context.User.Claims.FirstOrDefault(i => i.Type == "authorizations");

            var authorizations =
                JsonConvert.DeserializeObject<string[]>(
                    authorizationsList?.Value ?? throw new NullReferenceException("Authorizations claim"));

            // Check is the user is authorized to access the resources
            foreach (var authorization in authorizations)
            {
                if (requirement.Authorizations.Contains(authorization))
                {
                    context.Succeed(requirement);
                    break;
                }
            }

            return Task.CompletedTask;
        }
    }
}