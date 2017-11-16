using System.ComponentModel.DataAnnotations;
using MediatR;

namespace BoltJwt.Application.Commands.Authorizations
{
    public class AuthorizationDeleteCommand : IRequest<bool>
    {
        [Required]
        public int Id { get; set; }
    }
}