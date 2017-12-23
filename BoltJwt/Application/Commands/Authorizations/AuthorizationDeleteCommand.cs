using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BoltJwt.Application.Commands.Authorizations
{
    public class AuthorizationDeleteCommand : IRequest<bool>
    {
        [Required]
        [FromQuery(Name = "id")]
        public int Id { get; set; }
    }
}