using MediatR;

namespace BoltJwt.Application.Commands.Users
{
    public class AddAuthorizationUserCommand : IRequest<bool>
    {
        public int UserId { get; set; }
        public int AuthorizationId { get; set; }
    }
}