using System.Collections.Generic;
using MediatR;

namespace BoltJwt.Application.Commands.Roles
{
    public class RoleInsertCommand : IRequest<bool>
    {
        public string Description { get; set; }

        public IEnumerable<string> Authorizations { get; set; }
    }
}