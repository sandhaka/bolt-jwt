using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace BoltJwt.Application.Middlewares.Authentication
{
    public class JwtRenewMiddleware : AuthenticationJwtMiddleware, IMiddleware
    {
        public JwtRenewMiddleware(RequestDelegate next, IOptions<TokenOptions> options) : base(next, options)
        {
        }

        protected override Task GenerateTokenAsync(HttpContext httpContext)
        {
            throw new System.NotImplementedException();
        }
    }
}