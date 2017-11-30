using System.Threading.Tasks;
using BoltJwt.Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BoltJwt.Application.Middlewares.Authentication
{
    /// <summary>
    /// Base class for auth jwt middlewares
    /// </summary>
    public abstract class AuthenticationJwtMiddleware
    {
        /// <summary>
        /// Delegate to the next middleware
        /// </summary>
        private readonly RequestDelegate _next;
        /// <summary>
        /// Token options
        /// </summary>
        protected readonly TokenOptions Options;
        /// <summary>
        /// Json serialization settings
        /// </summary>
        protected readonly JsonSerializerSettings SerializerSettings;

        protected AuthenticationJwtMiddleware(RequestDelegate next, IOptions<TokenOptions> options)
        {
            _next = next;
            Options = options.Value;
            SerializerSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            };
        }

        /// <summary>
        /// Analyze a request - Return a new access token or delegate the request to the next middleware
        /// </summary>
        /// <param name="httpContext">Http context</param>
        /// <param name="context">DbContext</param>
        /// <returns>Return the next task</returns>
        public virtual Task Invoke(HttpContext httpContext, IdentityContext context)
        {
            if (!httpContext.Request.Method.Equals("POST"))
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                return httpContext.Response.WriteAsync("Bad request.");
            }

            return GenerateTokenAsync(httpContext, context);
        }

        /// <summary>
        /// Implementation specific
        /// </summary>
        /// <param name="context">DbContext</param>
        /// <param name="httpContext">Http context</param>
        /// <returns>Task</returns>
        protected abstract Task GenerateTokenAsync(HttpContext httpContext, IdentityContext context);
    }
}