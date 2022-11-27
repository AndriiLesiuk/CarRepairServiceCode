using System.Net;
using System.Threading.Tasks;
using CarRepairServiceCode.RequestModels.EmpPosition;
using Xunit;

namespace CarRepairServiceCode.IntegrationTests.EmpPositionsTests
{
    public class EmpPositionTests : IClassFixture<EmpPositionFixture>
    {
        private readonly EmpPositionFixture _factory;

        public EmpPositionTests(EmpPositionFixture factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task AddPosition_ValidEmpPositionRequest_ReturnSuccess()
        {
            // Act
            var response = await _factory.AddPosition(_factory.EmpPositionRequestPainter, _factory.TokenWithPermissions);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(_factory.EmpPositionViewPainter.PositionName, response.View.PositionName);
        }

        [Fact]
        public async Task AddPosition_EmptyEmpPositionRequest_ReturnBadRequest()
        {
            // Act
            var response = await _factory.AddPosition(new EmpPositionRequest(), _factory.TokenWithPermissions);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task AddPosition_UserWithoutPermissions_ReturnPermissionException()
        {
            // Act
            var response = await _factory.AddPosition(_factory.EmpPositionRequestPainter, _factory.TokenWithoutPermissions);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task AddPosition_UnauthorizedUser_ReturnUnauthorizedException()
        {
            // Act
            var response = await _factory.AddPosition(_factory.EmpPositionRequestPainter, null);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetPositions_EmptyEmpPositionRequest_ReturnSuccess()
        {
            // Act
            var response = await _factory.GetPositions(_factory.EmptyQuery, _factory.TokenWithPermissions);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Collection(response.View.GetRange(0,3), item => Assert.Contains(_factory.EmpPositionViewManager.PositionName, item.PositionName),
                item => Assert.Contains(_factory.EmpPositionViewMechanic.PositionName, item.PositionName),
                item => Assert.Contains(_factory.EmpPositionViewMaid.PositionName, item.PositionName));
        }

        [Fact]
        public async Task GetPositions_EmpPositionRequestWithExistQuery_ReturnSuccess()
        {
            // Act
            var response = await _factory.GetPositions(_factory.ExistQuery, _factory.TokenWithPermissions);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(_factory.EmpPositionListForExistQuery, response.View.Count);
            Assert.Collection(response.View, item => Assert.Contains(_factory.EmpPositionViewManager.PositionName, item.PositionName),
                   item => Assert.Contains(_factory.EmpPositionViewMaid.PositionName, item.PositionName));
        }

        [Fact]
        public async Task GetPositions_EmpPositionRequestWithNonExistQuery_ReturnSuccess()
        {
            // Act
            var response = await _factory.GetPositions(_factory.NotExistQuery, _factory.TokenWithPermissions);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(_factory.EmpPositionEmptyListCount, response.View.Count);
        }

        [Fact]
        public async Task GetPositions_UserWithoutPermissions_ReturnPermissionException()
        {
            // Act
            var response = await _factory.GetPositions(_factory.NotExistQuery, _factory.TokenWithoutPermissions);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task GetPositions_UnauthorizedUser_ReturnUnauthorizedException()
        {
            // Act
            var response = await _factory.GetPositions(_factory.NotExistQuery, null);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetPositionById_EmpPositionRequestWithExistId_ReturnSuccess()
        {
            // Act
            var response = await _factory.GetPositionById(_factory.EmpPositionManagerId, _factory.TokenWithPermissions);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(_factory.EmpPositionViewManager.PositionName, response.View.PositionName);
        }

        [Fact]
        public async Task GetPositionById_EmpPositionRequestWithNotExistId_ReturnNotFoundException()
        {
            // Act
            var response = await _factory.GetPositionById(_factory.NotExistEmpPositionId, _factory.TokenWithPermissions);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetPositionById_UserWithoutPermissions_ReturnPermissionException()
        {
            // Act
            var response = await _factory.GetPositionById(_factory.EmpPositionManagerId, _factory.TokenWithoutPermissions);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task GetPositionById_UnauthorizedUser_ReturnUnauthorizedException()
        {
            // Act
            var response = await _factory.GetPositionById(_factory.EmpPositionManagerId, null);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task UpdatePosition_EmpPositionRequestWithExistId_ReturnSuccess()
        {
            // Act
            var response = await _factory.UpdatePosition(_factory.EmpPositionMechanicId,_factory.EmpPositionRequestPainter, _factory.TokenWithPermissions);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(response.View.PositionName, _factory.EmpPositionViewPainter.PositionName);
        }

        [Fact]
        public async Task UpdatePosition_EmptyEmpPositionRequest_ReturnBadRequest()
        {
            // Act
            var response = await _factory.UpdatePosition(_factory.EmpPositionMechanicId, new EmpPositionRequest(), _factory.TokenWithPermissions);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdatePosition_EmpPositionRequestWithNotExistId_ReturnNotFoundException()
        {
            // Act
            var response = await _factory.UpdatePosition(_factory.NotExistEmpPositionId, _factory.EmpPositionRequestPainter, _factory.TokenWithPermissions);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdatePosition_PassTheIDThatBelongsToTheManager_ReturnPermissionException()
        {
            // Act
            var response = await _factory.UpdatePosition(_factory.EmpPositionManagerId, _factory.EmpPositionRequestPainter, _factory.TokenWithoutPermissions);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task UpdatePosition_UserWithoutPermissions_ReturnPermissionException()
        {
            // Act
            var response = await _factory.UpdatePosition(_factory.EmpPositionMechanicId, _factory.EmpPositionRequestPainter, _factory.TokenWithoutPermissions);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task UpdatePosition_UnauthorizedUser_ReturnUnauthorizedException()
        {
            // Act
            var response = await _factory.UpdatePosition(_factory.EmpPositionMechanicId, _factory.EmpPositionRequestPainter, null);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task DeletePosition_EmpPositionRequestWithExistId_ReturnNoContent()
        {
            // Act
            var response = await _factory.DeletePosition(_factory.EmpPositionAccountantId, _factory.TokenWithPermissions);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeletePosition_EmpPositionRequestWithNotExistId_ReturnNotFoundException()
        {
            // Act
            var response = await _factory.DeletePosition(_factory.NotExistEmpPositionId, _factory.TokenWithPermissions);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeletePosition_PassTheIDThatBelongsToTheManager_ReturnPermissionException()
        {
            // Act
            var response = await _factory.DeletePosition(_factory.EmpPositionManagerId, _factory.TokenWithPermissions);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task DeletePosition_UserWithoutPermissions_ReturnPermissionException()
        {
            // Act
            var response = await _factory.DeletePosition(_factory.EmpPositionAccountantId, _factory.TokenWithoutPermissions);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task DeletePosition_UnauthorizedUser_ReturnUnauthorizedException()
        {
            // Act
            var response = await _factory.DeletePosition(_factory.EmpPositionAccountantId, null);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
