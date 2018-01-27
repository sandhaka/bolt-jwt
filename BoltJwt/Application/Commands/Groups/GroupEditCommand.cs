using System.ComponentModel.DataAnnotations;
using MediatR;

namespace BoltJwt.Application.Commands.Groups
{
    public class GroupEditCommand: IRequest<bool>
    {
        [Required]
        public int Id { get; set; }
        public string Description { get; set; }
    }
}