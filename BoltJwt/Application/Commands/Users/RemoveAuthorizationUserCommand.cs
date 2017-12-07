using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BoltJwt.Application.Commands.Users
{
    public class RemoveAuthorizationUserCommand : IRequest<bool>
    {
        [FromQuery(Name = "userId")]
        public int UserId { get; set; }
        [FromQuery(Name = "authorizations")]
        public string Authorizations { get; set; }
    }
}