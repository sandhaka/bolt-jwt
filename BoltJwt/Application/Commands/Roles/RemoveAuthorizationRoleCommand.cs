using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BoltJwt.Application.Commands.Roles
{
    public class RemoveAuthorizationRoleCommand: IRequest<bool>
    {
        [FromQuery(Name = "roleId")]
        [Required]
        public int RoleId { get; set; }
        [FromQuery(Name = "authorizations")]
        [Required]
        public string Authorizations { get; set; }
    }
}