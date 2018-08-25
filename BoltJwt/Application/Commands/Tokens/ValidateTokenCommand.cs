using MediatR;

namespace BoltJwt.Application.Commands.Tokens
{
    public class ValidateTokenCommand: IRequest<bool>
    {
        /// <summary>
        /// Request id just for logging
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Token to validate
        /// </summary>
        public string Token { get; set; }
    }
}