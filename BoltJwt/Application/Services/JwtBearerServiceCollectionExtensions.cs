using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace BoltJwt.Application.Services
{
    /// <summary>
    /// A extensions collection
    /// </summary>
    public static class JwtBearerServiceCollectionExtensions
    {
        /// <summary>
        /// Add Jwt bearer through ServiceCollection interface
        /// </summary>
        /// <param name="services">Services collection</param>
        /// <param name="configuration">Configuration</param>
        /// <returns>Services collection</returns>
        public static IServiceCollection AddJwtBearerAuthentication(this IServiceCollection services,
            IConfiguration configuration)
        {
            var pubKeyPath = configuration.GetSection("Secrets:PublicKey").Value;

            var publicKey = new X509Certificate2(pubKeyPath).GetRSAPublicKey();

            services.AddAuthentication().AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        // Add the access_token as a claim, as we may actually need it
                        if (context.SecurityToken is JwtSecurityToken accessToken)
                        {
                            if (context.Principal.Identity is ClaimsIdentity identity)
                            {
                                identity.AddClaim(new Claim("access_token", accessToken.RawData));
                            }
                        }

                        return Task.CompletedTask;
                    }
                };
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new RsaSecurityKey(publicKey),
                    ValidateIssuer = true,
                    ValidIssuer = configuration.GetSection("Jwt:Issuer").Value,
                    ValidateAudience = true,
                    ValidAudience = configuration.GetSection("Jwt:Audience").Value,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            return services;
        }
    }
}