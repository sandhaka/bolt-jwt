﻿using MediatR;

namespace BoltJwt.Application.Commands.Users
{
    public class UserEditCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }
    }
}