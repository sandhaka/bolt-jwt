using MediatR;

namespace BoltJwt.Application.Commands.Users
{
    public class UserDeleteCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}