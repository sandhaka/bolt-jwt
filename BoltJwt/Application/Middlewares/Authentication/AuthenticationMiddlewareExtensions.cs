using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using BoltJwt.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BoltJwt.Application.Middlewares.Authentication
{
    public static class AuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseJwtProvider(this IApplicationBuilder builder, IServiceProvider services,
            IConfiguration configuration)
        {
            // Get the private key
            var pfxPath = "certs/dev.boltjwt.pfx";
            var prvtKeyPassphrase = File.ReadAllText("certs/dev.boltjwt.passphrase");
            var privateKey = new X509Certificate2(pfxPath, prvtKeyPassphrase).GetRSAPrivateKey();

            // Use authentication middleware
            builder.UseAuthentication();

            // Setup Token provider
            var tokenProviderOptions = new TokenProviderOptions()
            {
                Path = "/api/token",
                Issuer = configuration.GetSection("Jwt:Issuer").Value,
                Audience = configuration.GetSection("Jwt:Audience").Value,
                SigningCredentials = new SigningCredentials(new RsaSecurityKey(privateKey), SecurityAlgorithms.RsaSha256),
                IdentityResolver = UserRepository.GetIdentityAsync
            };

            builder.UseMiddleware<JwtProviderMiddleware>(Options.Create(tokenProviderOptions));

            // Setup Token renew options
            var tokenRenewOptions = new TokenOptions
            {
                Path = "/api/tokenrenew"
            };

            return builder.UseMiddleware<JwtRenewMiddleware>(Options.Create(tokenRenewOptions));
        }
    }
}