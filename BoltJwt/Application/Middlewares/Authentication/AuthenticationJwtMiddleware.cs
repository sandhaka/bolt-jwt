using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BoltJwt.Application.Middlewares.Authentication
{
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
        /// <param name="context">Http context</param>
        /// <returns>Return the next task</returns>
        public virtual Task Invoke(HttpContext context)
        {
            if (!context.Request.Path.Equals(Options.Path, StringComparison.Ordinal))
            {
                return _next(context);
            }

            if (!context.Request.Method.Equals("POST"))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return context.Response.WriteAsync("Bad request.");
            }

            return GenerateTokenAsync(context);
        }

        /// <summary>
        /// Implementation specific
        /// </summary>
        /// <param name="httpContext">Http context</param>
        /// <returns>Task</returns>
        protected abstract Task GenerateTokenAsync(HttpContext httpContext);
    }
}