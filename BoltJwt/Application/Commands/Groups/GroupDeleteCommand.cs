using System.ComponentModel.DataAnnotations;
using MediatR;

namespace BoltJwt.Application.Commands.Groups
{
    public class GroupDeleteCommand: IRequest<bool>
    {
        [Required]
        public int Id { get; set; }
    }
}