using System;
using System.Security.Cryptography.X509Certificates;
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
            var pubKeyPath = "certs/dev.boltjwt.crt";

            var publicKey = new X509Certificate2(pubKeyPath).GetRSAPublicKey();

            services
                .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
            {
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