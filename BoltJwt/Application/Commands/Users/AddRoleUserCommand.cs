using MediatR;

namespace BoltJwt.Application.Commands.Users
{
    public class AddRoleUserCommand : IRequest<bool>
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}