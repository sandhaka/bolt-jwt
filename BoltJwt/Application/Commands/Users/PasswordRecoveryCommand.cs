using MediatR;

namespace BoltJwt.Application.Commands.Users
{
    public class PasswordRecoveryCommand : IRequest<bool>
    {
        public string Email { get; set; }
    }
}