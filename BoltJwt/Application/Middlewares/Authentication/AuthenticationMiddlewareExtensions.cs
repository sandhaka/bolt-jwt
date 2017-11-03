using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using BoltJwt.Model.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BoltJwt.Application.Middlewares.Authentication
{
    public static class AuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseJwtProvider(this IApplicationBuilder builder, IServiceProvider services,
            IConfiguration configuration)
        {
            var userRepository = services.GetRequiredService<IUserRepository>();

            var pfxPath = configuration.GetSection("Secrets:pfxCert").Value;
            var prvtKeyPassphrase = File.ReadAllText(configuration.GetSection("Secrets:PrvtKeyPassphrase").Value);

            var privateKey = new X509Certificate2(pfxPath, prvtKeyPassphrase).GetRSAPrivateKey();

            // Setup Token provider
            var tokenProviderOptions = new TokenProviderOptions()
            {
                Path = "/api/token",
                Issuer = configuration.GetSection("Jwt:Issuer").Value,
                Audience = configuration.GetSection("Jwt:Audience").Value,
                SigningCredentials = new SigningCredentials(new RsaSecurityKey(privateKey), SecurityAlgorithms.RsaSha256),
                IdentityResolver = userRepository.GetIdentityAsync
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