using System.Threading;
using System.Threading.Tasks;
using MimeKit;

namespace BoltJwt.Application.Services
{
    public interface IMailService
    {
        Task SendAnEmailAsync(MimeMessage message, CancellationToken cancellationToken);
    }
}