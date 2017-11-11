using System.ComponentModel.DataAnnotations;
using MediatR;

namespace BoltJwt.Application.Commands.Users
{
    public class UserDeleteCommand : IRequest<bool>
    {
        [Required]
        public int Id { get; set; }
    }
}