using System.Collections.Generic;
using MediatR;

namespace BoltJwt.Application.Commands.Users
{
    public class EditUserGroupsCommand : IRequest<bool>
    {
        public int UserId { get; set; }
        public IEnumerable<int> Groups { get; set; }
    }
}