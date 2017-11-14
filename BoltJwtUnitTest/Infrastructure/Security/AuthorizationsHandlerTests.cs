using System.Collections.Generic;
using System.Security.Claims;
using BoltJwt.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Xunit;

namespace BoltJwtUnitTest.Infrastructure.Security
{
    public class AuthorizationsHandlerTests
    {
        [Fact]
        public void AuthorizationHandlerSuccessTest()
        {
            // Arrange
            AuthorizationHandlerContext authorizationHandlerContext = new AuthorizationHandlerContext(
                new List<IAuthorizationRequirement>{ new AuthorizationsRequirement("test-auth")},
                new ClaimsPrincipal(
                    new ClaimsIdentity(
                        new List<Claim>
                        {
                            new Claim("isRoot", "false"),
                            new Claim("authorizations", JsonConvert.SerializeObject(new[] {"test-auth"})),
                        })
                    ),
                new object()
                );
            AuthorizationsHandler authorizationsHandlerSut = new AuthorizationsHandler();

            // Act
            authorizationsHandlerSut.HandleAsync(authorizationHandlerContext).Wait();

            // Verify
            Assert.True(authorizationHandlerContext.HasSucceeded);
        }

        [Fact]
        public void AuthorizationHandlerFailureTest()
        {
            // Arrange
            AuthorizationHandlerContext authorizationHandlerContext = new AuthorizationHandlerContext(
                new List<IAuthorizationRequirement>{ new AuthorizationsRequirement("test-auth")},
                new ClaimsPrincipal(
                    new ClaimsIdentity(
                        new List<Claim>
                        {
                            new Claim("isRoot", "false"),
                            new Claim("authorizations", JsonConvert.SerializeObject(new string[]{})),
                        })
                ),
                new object()
            );
            AuthorizationsHandler authorizationsHandlerSut = new AuthorizationsHandler();

            // Act
            authorizationsHandlerSut.HandleAsync(authorizationHandlerContext).Wait();

            // Verify
            Assert.False(authorizationHandlerContext.HasSucceeded);
        }

        [Fact]
        public void AuthorizationHandlerRootTest()
        {
            // Arrange
            AuthorizationHandlerContext authorizationHandlerContext = new AuthorizationHandlerContext(
                new List<IAuthorizationRequirement>{ new AuthorizationsRequirement("test-auth")},
                new ClaimsPrincipal(
                    new ClaimsIdentity(
                        new List<Claim>
                        {
                            new Claim("isRoot", "true")
                        })
                ),
                new object()
            );
            AuthorizationsHandler authorizationsHandlerSut = new AuthorizationsHandler();

            // Act
            authorizationsHandlerSut.HandleAsync(authorizationHandlerContext).Wait();

            // Verify
            Assert.True(authorizationHandlerContext.HasSucceeded);
        }
    }
}