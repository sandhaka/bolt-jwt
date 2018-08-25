using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Infrastructure.Security;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BoltJwt.Application.Commands.Tokens.Handlers
{
    public class ValidateTokenCommandHandler: IRequestHandler<ValidateTokenCommand, bool>
    {
        private readonly IConfiguration _configuration;

        public ValidateTokenCommandHandler(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<bool> Handle(ValidateTokenCommand request, CancellationToken cancellationToken)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var publicKey = new X509Certificate2(
                CertsProviderService.GetCertificatePath(),
                CertsProviderService.GetCertificatePassphrase()).GetRSAPublicKey();

            var principal = jwtSecurityTokenHandler.ValidateToken(
                request.Token,
                new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new RsaSecurityKey(publicKey),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration.GetSection("Jwt:Issuer").Value,
                    ValidateAudience = true,
                    ValidAudience = _configuration.GetSection("Jwt:Audience").Value,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                },
                out _);

            // Implicit validated
            return true;
        }
    }
}