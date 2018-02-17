using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using BoltJwt.Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BoltJwt.Application.Middlewares.Authentication
{
    public class JwtRenewMiddleware : AuthenticationJwtMiddleware
    {
        public JwtRenewMiddleware(RequestDelegate next, IOptions<TokenOptions> options) : base(next, options)
        {
            ThrowIfInvalidOptions(Options);
        }

        public override Task Invoke(HttpContext httpContext, IdentityContext context)
        {
            // Token renew require an authenticated user
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return httpContext.Response.WriteAsync("Unauthorized.");
            }

            return GenerateTokenAsync(httpContext, context);
        }

        protected override async Task GenerateTokenAsync(HttpContext httpContext, IdentityContext dbContext)
        {
            // Retrieve the access token, skip 'Bearer ' string
            var encodedToken = httpContext.Request.Headers["Authorization"].ToString().Substring(7);

            var tokenHandler = new JwtSecurityTokenHandler();

            var opts = ((TokenProviderOptions) Options);

            var token = tokenHandler.ReadJwtToken(encodedToken);

            // Update token duration (+ 1 week)
            var jwt = new JwtSecurityToken(
                issuer: opts.Issuer,
                audience: opts.Audience,
                claims: token.Claims,
                notBefore: null,
                expires: DateTime.UtcNow.Add(opts.Expiration).ToUniversalTime(),
                signingCredentials: opts.SigningCredentials
            );

            var encodedJwt = tokenHandler.WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                expires_in = (int)Options.Expiration.TotalMilliseconds
            };

            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(response, SerializerSettings));
        }

        /// <summary>
        /// Check the options content
        /// </summary>
        /// <param name="options"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        private void ThrowIfInvalidOptions(TokenOptions options)
        {
            if (options.Expiration == TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(TokenOptions.Expiration));
            }
        }
    }
}