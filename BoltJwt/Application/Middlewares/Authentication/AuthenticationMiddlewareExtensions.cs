using System;
using System.Security.Cryptography.X509Certificates;
using BoltJwt.Infrastructure.Repositories;
using BoltJwt.Infrastructure.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
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
            var privateKey = new X509Certificate2(
                CertsProviderService.GetCertificatePath(),
                CertsProviderService.GetCertificatePassphrase()).GetRSAPrivateKey();

            // Use authentication middleware
            builder.UseAuthentication();

            // Setup Token provider
            var tokenProviderOptions = new TokenProviderOptions()
            {
                Issuer = configuration.GetSection("Jwt:Issuer").Value,
                Audience = configuration.GetSection("Jwt:Audience").Value,
                SigningCredentials = new SigningCredentials(new RsaSecurityKey(privateKey), SecurityAlgorithms.RsaSha256),
                IdentityResolver = UserRepository.GetIdentityAsync
            };

            builder.Map(
                new PathString("/api/token"),
                a => a.UseMiddleware<JwtProviderMiddleware>(Options.Create(tokenProviderOptions))
            );

            return builder.Map(
                new PathString("/api/tokenrenew"),
                a => a.UseMiddleware<JwtRenewMiddleware>(Options.Create(tokenProviderOptions))
            );
        }
    }
}