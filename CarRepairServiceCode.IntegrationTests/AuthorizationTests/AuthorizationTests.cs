using System.Net;
using System.Threading.Tasks;
using CarRepairServiceCode.RequestModels.Authorization;
using Xunit;

namespace CarRepairServiceCode.IntegrationTests.AuthorizationTests
{
    public class AuthorizationTests : IClassFixture<AuthorizationFixture>
    {
        private readonly AuthorizationFixture _factory;

        public AuthorizationTests(AuthorizationFixture factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GenerateTokenForEmployee_AuthRequestRequestWithExistCredentials_ReturnSuccess()
        {
            // Act
            var response = await _factory.GenerateTokenForEmployee(_factory.AuthRequestWithExistCredentials);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GenerateTokenForEmployee_AuthRequestRequestWithNotExistCredentials_ReturnUnauthorized()
        {
            // Act
            var response = await _factory.GenerateTokenForEmployee(_factory.AuthRequestWithNotExistCredentials);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GenerateTokenForEmployee_AuthRequestRequestWithoutCredentials_ReturnBadRequest()
        {
            // Act
            var response = await _factory.GenerateTokenForEmployee(new AuthRequest());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task RefreshToken_ValidToken_ReturnSuccess()
        {
            // Act
            var response = await _factory.RefreshTokenForEmployee(_factory.AuthRequestWithExistCredentials, _factory.TokenWithPermissions);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task RefreshToken_ExpiredToken_ReturnUnauthorized()
        {
            // Act
            var response = await _factory.RefreshTokenForEmployee(_factory.AuthRequestWithExistCredentials, _factory.ExpiredToken);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task RefreshToken_InvalidToken_ReturnUnauthorized()
        {
            // Act
            var response = await _factory.RefreshTokenForEmployee(_factory.AuthRequestWithExistCredentials, _factory.InvalidToken);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
