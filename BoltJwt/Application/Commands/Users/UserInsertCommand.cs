using MediatR;

namespace BoltJwt.Application.Commands.Users
{
    public class UserInsertCommand : IRequest<bool>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}