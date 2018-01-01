using MediatR;

namespace BoltJwt.Application.Commands.Users
{
    public class ResetPasswordCommand : IRequest<bool>
    {
        public int UserId { get; set; }
        public string Code { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}