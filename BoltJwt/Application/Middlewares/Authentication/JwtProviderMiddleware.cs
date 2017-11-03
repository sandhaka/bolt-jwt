using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BoltJwt.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BoltJwt.Application.Middlewares.Authentication
{
    public class JwtProviderMiddleware : AuthenticationJwtMiddleware, IMiddleware
    {
        public JwtProviderMiddleware(RequestDelegate next, IOptions<TokenOptions> options) : base(next, options)
        {
            ThrowIfInvalidOptions(Options);
        }

        protected override async Task GenerateTokenAsync(HttpContext httpContext)
        {
            var jsonPost = JObject.Parse(await httpContext.Request.GetRawBodyStringAsync(Encoding.UTF8));

            var username = jsonPost["username"].Value<string>();
            var password = jsonPost["password"].Value<string>();

            var opts = ((TokenProviderOptions) Options);

            if(opts == null)
                throw new InvalidCastException("TokenProviderOptions");

            var identity = await opts.IdentityResolver(username, password);

            if (identity == null)
            {
                httpContext.Response.StatusCode = 400;
                await httpContext.Response.WriteAsync("Invalid username or password.");
                return;
            }

            var encodedJwt = await CreateJwtAsync(opts, identity, username);

            var response = new
            {
                access_token = encodedJwt,
                expires_in = (int)opts.Expiration.TotalSeconds
            };

            // Serialize and return the response
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsync(
                JsonConvert.SerializeObject(response, SerializerSettings));
        }

        private async Task<string> CreateJwtAsync(TokenProviderOptions options, ClaimsIdentity identity, string username)
        {
            var now = DateTime.UtcNow;

            var issuedAt = new DateTimeOffset(now).ToUniversalTime().ToUnixTimeSeconds().ToString();

            // Add public claims
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Iss, options.Issuer),
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Aud, options.Audience),
                new Claim(JwtRegisteredClaimNames.Iat, issuedAt, ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Jti, await options.NonceGenerator())
            };

            // Add identity claims
            claims.AddRange(identity.Claims);

            // Create the JWT and write it to a string
            var jwt = new JwtSecurityToken(
                issuer: options.Issuer,
                audience: options.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(options.Expiration),
                signingCredentials: options.SigningCredentials);

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private void ThrowIfInvalidOptions(TokenOptions options)
        {
            if (!(options is TokenProviderOptions currentTokenOptions))
            {
                throw new NullReferenceException("Wrong token provider options");
            }

            if (string.IsNullOrEmpty(options.Path))
            {
                throw new ArgumentNullException(nameof(TokenOptions.Path));
            }
            if (options.Expiration == TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(TokenOptions.Expiration));
            }
            if (currentTokenOptions.IdentityResolver == null)
            {
                throw new ArgumentNullException(nameof(currentTokenOptions.IdentityResolver));
            }
            if (currentTokenOptions.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(currentTokenOptions.SigningCredentials));
            }
            if (currentTokenOptions.NonceGenerator == null)
            {
                throw new ArgumentNullException(nameof(currentTokenOptions.NonceGenerator));
            }
            if (string.IsNullOrEmpty(currentTokenOptions.Issuer))
            {
                throw new ArgumentNullException(nameof(currentTokenOptions.Issuer));
            }
            if (string.IsNullOrEmpty(currentTokenOptions.Audience))
            {
                throw new ArgumentNullException(nameof(currentTokenOptions.Audience));
            }
        }
    }
}