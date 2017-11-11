using System.ComponentModel.DataAnnotations;
using MediatR;

namespace BoltJwt.Application.Commands.Roles
{
    public class RoleDeleteCommand : IRequest<bool>
    {
        [Required]
        public int Id { get; set; }
    }
}