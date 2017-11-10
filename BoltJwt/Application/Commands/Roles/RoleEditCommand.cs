using MediatR;

namespace BoltJwt.Application.Commands.Roles
{
    public class RoleEditCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }
}