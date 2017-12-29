using System;
using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Application.Commands.Users;
using BoltJwt.Application.Commands.Users.Handlers;
using BoltJwt.Application.Commands.Users.Responses;
using BoltJwt.Controllers.Dto;
using BoltJwt.Domain.Model;
using BoltJwt.Domain.Model.Abstractions;
using MediatR;
using Moq;
using Xunit;

namespace BoltJwtUnitTest.Application.Commands
{
    public class UserCommandsTests
    {
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IUnitOfWork> _unitOfwork;

        public UserCommandsTests()
        {
            _unitOfwork = new Mock<IUnitOfWork>();
            _userRepository = new Mock<IUserRepository>();

            _userRepository.Setup(x => x.UnitOfWork).Returns(_unitOfwork.Object);
            _unitOfwork.Setup(x => x.SaveEntitiesAsync(new CancellationToken())).Returns(Task.FromResult(true));
            _userRepository.Setup(x => x.UserNameExistsAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            _userRepository.Setup(x => x.EmailExistsAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
        }

        [Fact]
        public void InsertTest()
        {
            // Arrange
            _userRepository.Setup(x => x.Add(It.IsAny<User>()));

            IRequestHandler<UserInsertCommand, bool> commandHandlerSut =
                new UserInsertCommandHandler(_userRepository.Object);

            // Act
            commandHandlerSut.Handle(new UserInsertCommand
            {
                Email = "xxx",
                Name = "xxx",
                Surname = "xxx",
                UserName = "xxx"
            }, CancellationToken.None);

            // Verify
            _userRepository.Verify(x => x.UserNameExistsAsync("xxx"), Times.Exactly(1));
            _userRepository.Verify(x => x.EmailExistsAsync("xxx"), Times.Exactly(1));
            _userRepository.Verify(x => x.Add(It.IsAny<User>()), Times.Exactly(1));
            _unitOfwork.Verify(x => x.SaveEntitiesAsync(It.IsAny<CancellationToken>()), Times.Exactly(1));
        }

        [Fact]
        public void UpdateTest()
        {
            // Arrange
            _userRepository.Setup(x => x.UpdateAsync(It.IsAny<UserEditDto>())).Returns(Task.FromResult(new User()));

            IRequestHandler<UserEditCommand, UserEditResponse> commandHandlerSut =
                new UserEditCommandHandler(_userRepository.Object);

            // Act
            commandHandlerSut.Handle(new UserEditCommand
            {
                Id = 1,
                Name = "xxx",
                Surname = "xxx",
                UserName = "xxx"
            }, CancellationToken.None);

            // Verify
            _userRepository.Verify(x => x.UpdateAsync(It.IsAny<UserEditDto>()), Times.Exactly(1));
            _unitOfwork.Verify(x => x.SaveEntitiesAsync(It.IsAny<CancellationToken>()), Times.Exactly(1));
        }
    }
}