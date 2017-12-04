using System.ComponentModel.DataAnnotations;
using BoltJwt.Application.Commands.Users.Responses;
using MediatR;

namespace BoltJwt.Application.Commands.Users
{
    public class UserEditCommand : IRequest<UserEditResponse>
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }
    }
}