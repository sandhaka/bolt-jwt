using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BoltJwt.Application.Middlewares.Authentication
{
    public interface IMiddleware
    {
        Task Invoke(HttpContext context);
    }
}