using System.Collections.Generic;
using MediatR;

namespace BoltJwt.Application.Commands.Roles
{
    public class AddAuthorizationRoleCommand: IRequest<bool>
    {
        public int RoleId { get; set; }
        public IEnumerable<int> Authorizations { get; set; }
    }
}