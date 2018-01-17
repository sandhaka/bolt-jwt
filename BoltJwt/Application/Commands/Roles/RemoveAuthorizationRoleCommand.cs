using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BoltJwt.Application.Commands.Roles
{
    public class RemoveAuthorizationRoleCommand: IRequest<bool>
    {
        [FromQuery(Name = "roleId")]
        public int RoleId { get; set; }
        [FromQuery(Name = "authorizations")]
        public string Authorizations { get; set; }
    }
}