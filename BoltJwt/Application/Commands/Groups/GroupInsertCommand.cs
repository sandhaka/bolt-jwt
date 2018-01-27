using MediatR;

namespace BoltJwt.Application.Commands.Groups
{
    public class GroupInsertCommand : IRequest<bool>
    {
        public string Description { get; set; }
    }
}