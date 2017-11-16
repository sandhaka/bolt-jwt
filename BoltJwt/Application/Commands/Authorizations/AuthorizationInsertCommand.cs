using MediatR;

namespace BoltJwt.Application.Commands.Authorizations
{
    public class AuthorizationInsertCommand : IRequest<bool>
    {
        public string Name { get; set; }
    }
}