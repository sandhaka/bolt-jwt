using System.Collections.Generic;
using MediatR;

namespace BoltJwt.Application.Commands.Users
{
    public class AddAuthorizationUserCommand : IRequest<bool>
    {
        public int UserId { get; set; }
        public IEnumerable<int> Authorizations { get; set; }
    }
}