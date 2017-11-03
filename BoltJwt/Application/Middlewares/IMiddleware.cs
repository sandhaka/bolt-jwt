using System.Threading.Tasks;
using BoltJwt.Infrastructure.Context;
using Microsoft.AspNetCore.Http;

namespace BoltJwt.Application.Middlewares
{
    public interface IMiddleware
    {
        Task Invoke(HttpContext httpContext, IdentityContext dbContext);
    }
}