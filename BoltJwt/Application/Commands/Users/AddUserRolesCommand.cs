using System.Collections.Generic;
using MediatR;

namespace BoltJwt.Application.Commands.Users
{
    public class AddUserRolesCommand : IRequest<bool>
    {
        public int UserId { get; set; }
        public IEnumerable<int> Roles { get; set; }
    }
}