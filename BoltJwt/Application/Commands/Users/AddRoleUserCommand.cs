using System.ComponentModel.DataAnnotations;
using MediatR;

namespace BoltJwt.Application.Commands.Users
{
    public class AddRoleUserCommand : IRequest<bool>
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int RoleId { get; set; }
    }
}