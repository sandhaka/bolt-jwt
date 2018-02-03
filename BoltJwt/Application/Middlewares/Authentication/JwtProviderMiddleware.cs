using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BoltJwt.Application.Services;
using BoltJwt.Domain.Model;
using BoltJwt.Domain.Model.Abstractions;
using BoltJwt.Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BoltJwt.Application.Middlewares.Authentication
{
    public class JwtProviderMiddleware : AuthenticationJwtMiddleware
    {
        private readonly ITokenLogsRepository _tokenLogsRepository;
        private readonly ILogger<JwtProviderMiddleware> _logger;

        public JwtProviderMiddleware(
            RequestDelegate next,
            IOptions<TokenOptions> options,
            ITokenLogsRepository tokenLogsRepository,
            ILogger<JwtProviderMiddleware> logger
            ) : base(next, options)
        {
            _tokenLogsRepository = tokenLogsRepository ?? throw new ArgumentNullException(nameof(tokenLogsRepository));
            _logger = logger;

            ThrowIfInvalidOptions(Options);
        }

        /// <summary>
        /// Resolve user credentials and build a JWT
        /// </summary>
        /// <param name="httpContext">http context</param>
        /// <param name="context">db context</param>
        /// <returns>task</returns>
        /// <exception cref="InvalidCastException">Wrong token options exception</exception>
        protected override async Task GenerateTokenAsync(HttpContext httpContext, IdentityContext context)
        {
            // Access to the body of the request
            var bodyContent = await httpContext.Request.GetRawBodyStringAsync(Encoding.UTF8);
            var jsonPost = JObject.Parse(bodyContent);

            var username = jsonPost["username"].Value<string>();
            var password = jsonPost["password"].Value<string>();

            var opts = ((TokenProviderOptions) Options);

            if(opts == null)
                throw new InvalidCastException("TokenProviderOptions");

            // Check credentials
            var identity = await opts.IdentityResolver(context ,username, password);
            if (identity == null)
            {
                httpContext.Response.StatusCode = 400;
                await httpContext.Response.WriteAsync("Invalid username or password.");
                return;
            }

            var encodedJwt = await CreateJwtAsync(opts, identity, username);

            // Makes the expiration explicit in the response
            var response = new
            {
                access_token = encodedJwt,
                expires_in = (int)opts.Expiration.Ticks
            };

            try
            {
                // Log the created token
                var userId = int.Parse(identity.Claims.First(i => i.Type == "userId")?.Value ??
                                       throw new NullReferenceException("userId claim"));

                await _tokenLogsRepository.AddAsync(new TokenLog
                {
                    Timestamp = DateTime.Now,
                    UserId = userId,
                    Value = encodedJwt.Remove(6) + "..."
                });

                await _tokenLogsRepository.UnitOfWork.SaveEntitiesAsync();

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on token logs creation");
            }

            // Serialize and return
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

            // Add identity claims returned from the user respository
            // These claims contains the defined authorizations for the user
            claims.AddRange(identity.Claims);

            // Create the JWT and write it to a string
            var jwt = new JwtSecurityToken(
                issuer: options.Issuer,
                audience: options.Audience,
                claims: claims,
                notBefore: null,
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