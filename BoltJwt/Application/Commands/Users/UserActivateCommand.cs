using MediatR;

namespace BoltJwt.Application.Commands.Users
{
    public class UserActivateCommand : IRequest<bool>
    {
        public string Code { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}