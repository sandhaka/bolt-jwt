using System.Collections.Generic;
using MediatR;

namespace BoltJwt.Application.Commands.Groups
{
    public class EditGroupRolesCommand: IRequest<bool>
    {
        public int GroupId { get; set; }
        public IEnumerable<int> Roles { get; set; }
    }
}