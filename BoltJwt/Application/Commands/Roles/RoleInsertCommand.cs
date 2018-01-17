using MediatR;

namespace BoltJwt.Application.Commands.Roles
{
    public class RoleInsertCommand : IRequest<bool>
    {
        public string Description { get; set; }
    }
}