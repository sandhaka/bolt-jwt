using MediatR;

namespace BoltJwt.Application.Commands.Roles
{
    public class RoleDeleteCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}