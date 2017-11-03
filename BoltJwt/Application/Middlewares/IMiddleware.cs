using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BoltJwt.Application.Middlewares
{
    public interface IMiddleware
    {
        Task Invoke(HttpContext context);
    }
}