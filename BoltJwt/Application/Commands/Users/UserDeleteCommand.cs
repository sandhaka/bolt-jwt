using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BoltJwt.Application.Commands.Users
{
    public class UserDeleteCommand : IRequest<bool>
    {
        [Required]
        [FromQuery(Name = "id")]
        public int Id { get; set; }
    }
}