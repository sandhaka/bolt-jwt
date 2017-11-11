using System.ComponentModel.DataAnnotations;
using MediatR;

namespace BoltJwt.Application.Commands.Roles
{
    public class RoleEditCommand : IRequest<bool>
    {
        [Required]
        public int Id { get; set; }
        public string Description { get; set; }
    }
}