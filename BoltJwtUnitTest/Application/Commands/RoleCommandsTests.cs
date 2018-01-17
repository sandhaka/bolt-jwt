using System.Threading;
using System.Threading.Tasks;
using BoltJwt.Application.Commands.Roles;
using BoltJwt.Application.Commands.Roles.Handlers;
using BoltJwt.Domain.Model;
using BoltJwt.Domain.Model.Abstractions;
using MediatR;
using Moq;
using Xunit;

namespace BoltJwtUnitTest.Application.Commands
{
    public class RoleCommandsTests
    {
        private readonly Mock<IRoleRepository> _roleRepository;
        private readonly Mock<IAuthorizationRepository> _authorizationRespository;
        private readonly Mock<IUnitOfWork> _unitOfwork;

        public RoleCommandsTests()
        {
            _unitOfwork = new Mock<IUnitOfWork>();
            _authorizationRespository = new Mock<IAuthorizationRepository>();
            _roleRepository = new Mock<IRoleRepository>();

            _roleRepository.Setup(x => x.UnitOfWork).Returns(_unitOfwork.Object);
            _unitOfwork.Setup(x => x.SaveEntitiesAsync(new CancellationToken())).Returns(Task.FromResult(true));
        }

        [Fact]
        public void InsertTest()
        {
            // Arrange
            _roleRepository.Setup(x => x.Add(It.IsAny<Role>()));
            _authorizationRespository.Setup(x => x.GetByNameAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new DefinedAuthorization("fake_auth")));

            IRequestHandler<RoleInsertCommand, bool> commandHandlerSut =
                new RoleInsertCommandHandler(_roleRepository.Object, _authorizationRespository.Object);

            // Act
            commandHandlerSut.Handle(new RoleInsertCommand
            {
                Description = "fake_role"
            }, CancellationToken.None);

            // Verify
            _roleRepository.Verify(x => x.Add(It.Is<Role>(r =>
                r.Description.Equals("fake_role"))));
            _unitOfwork.Verify(x => x.SaveEntitiesAsync(It.IsAny<CancellationToken>()), Times.Exactly(1));
        }
    }
}