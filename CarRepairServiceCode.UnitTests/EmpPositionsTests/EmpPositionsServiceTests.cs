using AutoMapper;
using CarRepairServiceCode.Helper;
using CarRepairServiceCode.Helper.HelperInterfaces;
using CarRepairServiceCode.Repository.Interfaces;
using CarRepairServiceCode.Repository.Models;
using CarRepairServiceCode.RequestModels.Authorization;
using CarRepairServiceCode.RequestModels.EmpPosition;
using CarRepairServiceCode.Services;
using CarRepairServiceCode.Services.ServiceInterfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CarRepairServiceCode.UnitTests.EmpPositionsTests
{
    public class EmpPositionsServiceTests
    {
        private const int ExistingEmpIdManager = 1;
        private const int ExistingEmpIdMachinist = 2;
        private const int NonExistingEmpId = 99;
        private const string SpecificQuery = "Ma";
        private const string Manager = "Manager";
        private const string Machinist = "Mechanic";
        private const string Maid = "Maid";
        private const string Accountant = "Accountant";

        private readonly IEmpPositionsService _empPositionsService;
        private readonly Mock<IEmpPositionRepository> _empPositionRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ITokenHelper> _tokenHelperMock;
        private readonly Mock<ILogger<EmpPositionsService>> _loggerMock;
        private readonly Mock<IPermissionRepository> _permissionRepositoryMock;

        private static readonly EmpPosition _empPositionManager = new EmpPosition { PositionId = ExistingEmpIdManager, PositionName = Manager, CreatedById = ExistingEmpIdManager, CreatedDate = DateTime.UtcNow };
        private static readonly EmpPosition _empPositionMachinist = new EmpPosition { PositionId = ExistingEmpIdMachinist, PositionName = Machinist, CreatedById = ExistingEmpIdManager, CreatedDate = DateTime.UtcNow };
        private static readonly EmpPosition _empPositionMaid = new EmpPosition { PositionId = 3, PositionName = Maid, CreatedById = ExistingEmpIdManager, CreatedDate = DateTime.UtcNow };
        private static readonly EmpPosition _empPositionAccountant = new EmpPosition { PositionId = 4, PositionName = Accountant, CreatedById = ExistingEmpIdManager, CreatedDate = DateTime.UtcNow };
        private static string _empPositionEntityName = DbEntitiesName.EmpPosition.GetEnumDescription();

        private readonly EmpPositionView _empPositionViewManager = new EmpPositionView { PositionName = Manager };
        private readonly EmpPositionView _empPositionViewMachinist = new EmpPositionView { PositionName = Machinist };
        private readonly EmpPositionView _empPositionViewMaid = new EmpPositionView { PositionName = Maid };
        private readonly EmpPositionView _empPositionViewAccountant = new EmpPositionView { PositionName = Accountant };

        private readonly List<EmpPosition> _allEmpPositionsList = new List<EmpPosition>() { _empPositionManager, _empPositionMachinist, _empPositionMaid, _empPositionAccountant };
        private readonly List<EmpPosition> _empPositionsFilteredList = new List<EmpPosition>() { _empPositionManager, _empPositionMachinist, _empPositionMaid };

        private readonly EmpPositionRequest _empPositionRequest = new EmpPositionRequest { PositionName = Machinist };

        private readonly EmpPositionQuery _empPositionQuery = new EmpPositionQuery { PositionName = SpecificQuery };
        private readonly EmpPositionQuery _empPositionQueryEmpty = new EmpPositionQuery();

        private static readonly Permissions _accessAllowedPermissionsWithManagerPositionId = new Permissions
        {
            PermissionId = ExistingEmpIdManager,
            PositionId = ExistingEmpIdManager,
            EntityForAction = _empPositionEntityName,
            CreateEntry = true,
            ReadEntry = true,
            UpdateEntry = true,
            DeleteEntry = true,
            CreatedById = ExistingEmpIdManager,
            CreatedDate = DateTime.UtcNow
        };
        private static readonly Permissions _accessIsNotAllowedPermissionsWithMachinistPositionId = new Permissions
        {
            PermissionId = ExistingEmpIdMachinist,
            PositionId = ExistingEmpIdMachinist,
            EntityForAction = _empPositionEntityName,
            CreateEntry = false,
            ReadEntry = false,
            UpdateEntry = false,
            DeleteEntry = false,
            CreatedById = ExistingEmpIdManager,
            CreatedDate = DateTime.UtcNow
        };

        private readonly AuthView _authViewWithAccessPermissions = new AuthView
        {
            EmployeeId = ExistingEmpIdManager,
            FirstName = "Bob",
            LastName = "lastName",
            PositionId = ExistingEmpIdManager,
            IsActive = true,
            Token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3",
            Expiration = DateTime.UtcNow
        };
        private readonly AuthView _authViewWithNotAccessPermissions = new AuthView
        {
            EmployeeId = ExistingEmpIdMachinist,
            FirstName = "Bob",
            LastName = "lastName",
            PositionId = ExistingEmpIdMachinist,
            IsActive = true,
            Token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3",
            Expiration = DateTime.UtcNow
        };

        public EmpPositionsServiceTests()
        {
            _empPositionRepositoryMock = new Mock<IEmpPositionRepository>();
            _empPositionRepositoryMock.Setup(repo => repo.AddPosition(_empPositionMachinist))
                .Returns(Task.FromResult(_empPositionMachinist));
            _empPositionRepositoryMock.Setup(repo => repo.GetPositions(_empPositionQueryEmpty))
                .Returns(Task.FromResult(_allEmpPositionsList.AsEnumerable()));
            _empPositionRepositoryMock.Setup(repo => repo.GetPositions(_empPositionQuery))
                .Returns(Task.FromResult(_empPositionsFilteredList.AsEnumerable()));
            _empPositionRepositoryMock.Setup(repo => repo.GetPositionById(ExistingEmpIdManager))
                .Returns(Task.FromResult(_empPositionManager));
            _empPositionRepositoryMock.Setup(repo => repo.GetPositionById(NonExistingEmpId))
                .Returns(Task.FromResult(default(EmpPosition)));
            _empPositionRepositoryMock.Setup(repo => repo.GetPositionById(ExistingEmpIdManager))
                .Returns(Task.FromResult(_empPositionManager));
            _empPositionRepositoryMock.Setup(repo => repo.GetPositionById(ExistingEmpIdMachinist))
                .Returns(Task.FromResult(_empPositionMachinist));
            _empPositionRepositoryMock.Setup(repo => repo.UpdatePosition(_empPositionMachinist))
                .Returns(Task.FromResult(_empPositionMachinist));

            _mapperMock = new Mock<IMapper>();
            _mapperMock.Setup(map => map.Map<EmpPosition>(_empPositionRequest)).Returns(_empPositionMachinist);
            _mapperMock.Setup(map => map.Map<EmpPositionView>(_empPositionMachinist)).Returns(_empPositionViewMachinist);
            _mapperMock.Setup(map => map.Map<EmpPosition>(new EmpPositionRequest())).Returns(new EmpPosition());
            _mapperMock.Setup(map => map.Map<EmpPositionView>(new EmpPosition())).Returns(_empPositionViewManager);
            _mapperMock.Setup(map => map.Map(_empPositionRequest, _empPositionMachinist)).Returns(_empPositionMachinist);
            _mapperMock.Setup(map => map.Map<EmpPositionView>(_empPositionManager))
                .Returns(() => _empPositionViewManager);
            _mapperMock.Setup(map => map.Map<EmpPositionView>(_empPositionMachinist))
                .Returns(() => _empPositionViewMachinist);
            _mapperMock.Setup(map => map.Map<EmpPositionView>(_empPositionMaid))
                .Returns(() => _empPositionViewMaid);
            _mapperMock.Setup(map => map.Map<EmpPositionView>(_empPositionAccountant))
                .Returns(() => _empPositionViewAccountant);

            _tokenHelperMock = new Mock<ITokenHelper>();
            _tokenHelperMock.Setup(token => token.CreateAuthViewFromToken()).Returns(_authViewWithAccessPermissions);

            _loggerMock = new Mock<ILogger<EmpPositionsService>>();

            _permissionRepositoryMock = new Mock<IPermissionRepository>();
            _permissionRepositoryMock.Setup(repo => repo.ReturnPermissionInfoById(ExistingEmpIdMachinist, _empPositionEntityName)).Returns(Task.FromResult(_accessIsNotAllowedPermissionsWithMachinistPositionId));
            _permissionRepositoryMock.Setup(repo => repo.ReturnPermissionInfoById(ExistingEmpIdManager, _empPositionEntityName)).Returns(Task.FromResult(_accessAllowedPermissionsWithManagerPositionId));

            _empPositionsService = new EmpPositionsService(_empPositionRepositoryMock.Object, _mapperMock.Object, _tokenHelperMock.Object, _loggerMock.Object, _permissionRepositoryMock.Object);
        }

        [Fact]
        public async Task AddPosition_ValidEmpPositionRequest_ReturnCreatedEmpPosition()
        {
            //Act
            var actual = await _empPositionsService.AddPosition(_empPositionRequest);

            //Assert
            Assert.Equal(_empPositionRequest.PositionName, actual.PositionName);
            _permissionRepositoryMock.Verify(repo => repo.ReturnPermissionInfoById(ExistingEmpIdManager, _empPositionEntityName), Times.Once);
            _empPositionRepositoryMock.Verify(repo => repo.AddPosition(_empPositionMachinist), Times.Once);
        }

        [Fact]
        public async Task AddPosition_AccessNotAllowed_ReturnPermissionException()
        {
            //Arrange
            _tokenHelperMock.Setup(token => token.CreateAuthViewFromToken()).Returns(_authViewWithNotAccessPermissions);

            //Act & Assert
            await Assert.ThrowsAsync<Exceptions.PermissionException>(async () => await _empPositionsService
                .AddPosition(_empPositionRequest));
        }

        [Fact]
        public async Task GetPositions_EmptyRequestOrNullValue_ReturnAllEmpPositions()
        {
            //Act
            var actual = (await _empPositionsService.GetPositions(_empPositionQueryEmpty)).ToList();

            //Assert
            Assert.Equal(_allEmpPositionsList.Count(), actual.Count());
            Assert.Collection(actual, item => Assert.Contains(Manager, item.PositionName),
                item => Assert.Contains(Machinist, item.PositionName),
                item => Assert.Contains(Maid, item.PositionName),
                item => Assert.Contains(Accountant, item.PositionName));
            _permissionRepositoryMock.Verify(repo => repo.ReturnPermissionInfoById(ExistingEmpIdManager, _empPositionEntityName), Times.Once);
            _empPositionRepositoryMock.Verify(repo => repo.GetPositions(_empPositionQueryEmpty), Times.Once);
        }

        [Fact]
        public async Task GetPositions_FilteredQuery_ReturnSpecificEmpPositions()
        {
            //Act
            var actual = (await _empPositionsService.GetPositions(_empPositionQuery)).ToList();

            //Assert
            Assert.Equal(_empPositionsFilteredList.Count(), actual.Count());
            Assert.Collection(actual, item => Assert.Contains(Manager, item.PositionName),
                item => Assert.Contains(Machinist, item.PositionName),
                item => Assert.Contains(Maid, item.PositionName));
            _permissionRepositoryMock.Verify(repo => repo.ReturnPermissionInfoById(ExistingEmpIdManager, _empPositionEntityName), Times.Once);
            _empPositionRepositoryMock.Verify(repo => repo.GetPositions(_empPositionQuery), Times.Once);
        }

        [Fact]
        public async Task GetPositions_AccessNotAllowed_ReturnPermissionException()
        {
            //Arrange
            _tokenHelperMock.Setup(token => token.CreateAuthViewFromToken()).Returns(_authViewWithNotAccessPermissions);

            //Act & Assert
            await Assert.ThrowsAsync<Exceptions.PermissionException>(async () => (await _empPositionsService
                .GetPositions(_empPositionQuery)).ToList());
        }

        [Fact]
        public async Task GetPositionById_ExistingId_ReturnSpecificEmpPosition()
        {
            //Act
            var actual = await _empPositionsService.GetPositionById(ExistingEmpIdManager);

            //Assert
            Assert.Equal(_empPositionManager.PositionName, actual.PositionName);
            _permissionRepositoryMock.Verify(repo => repo.ReturnPermissionInfoById(ExistingEmpIdManager, _empPositionEntityName), Times.Once);
            _empPositionRepositoryMock.Verify(repo => repo.GetPositionById(ExistingEmpIdManager), Times.Once);
        }

        [Fact]
        public async Task GetPositionById_NonExistingId_ReturnNotFoundException()
        {
            //Act & Assert
            await Assert.ThrowsAsync<Exceptions.NotFoundException>(async () => await _empPositionsService
                .GetPositionById(NonExistingEmpId));
        }

        [Fact]
        public async Task GetPositionById_AccessNotAllowed_ReturnPermissionException()
        {
            //Arrange
            _tokenHelperMock.Setup(token => token.CreateAuthViewFromToken()).Returns(_authViewWithNotAccessPermissions);

            //Act & Assert
            await Assert.ThrowsAsync<Exceptions.PermissionException>(async () => await _empPositionsService
                .GetPositionById(ExistingEmpIdMachinist));
        }

        [Fact]
        public async Task UpdatePosition_AccessNotAllowed_ReturnPermissionException()
        {
            //Arrange
            _tokenHelperMock.Setup(token => token.CreateAuthViewFromToken()).Returns(_authViewWithNotAccessPermissions);

            //Act & Assert
            await Assert.ThrowsAsync<Exceptions.PermissionException>(async () => await _empPositionsService
                .UpdatePosition(ExistingEmpIdMachinist, new EmpPositionRequest()));
        }

        [Fact]
        public async Task UpdatePosition_PassTheIDThatBelongsToTheManager_ReturnPermissionException()
        {
            //Act & Assert
            await Assert.ThrowsAsync<Exceptions.PermissionException>(async () => await _empPositionsService
                .UpdatePosition(ExistingEmpIdManager, new EmpPositionRequest()));
        }

        [Fact]
        public async Task UpdatePosition_NonExistingId_ReturnNotFoundException()
        {
            //Act & Assert
            await Assert.ThrowsAsync<Exceptions.NotFoundException>(async () => await _empPositionsService
                .UpdatePosition(NonExistingEmpId, _empPositionRequest));
        }

        [Fact]
        public async Task UpdatePosition_ExistingId_ReturnUpdatedEmpPosition()
        {
            //Act
            await _empPositionsService.UpdatePosition(ExistingEmpIdMachinist, _empPositionRequest);

            //Assert
            _permissionRepositoryMock.Verify(repo => repo.ReturnPermissionInfoById(ExistingEmpIdManager, _empPositionEntityName), Times.Once);
            _empPositionRepositoryMock.Verify(repo => repo.GetPositionById(ExistingEmpIdMachinist), Times.Once);
            _empPositionRepositoryMock.Verify(repo => repo.UpdatePosition(_empPositionMachinist), Times.Once);
        }

        [Fact]
        public async Task DeletePosition_NonExistingId_ReturnNotFoundException()
        {
            //Act & Assert
            await Assert.ThrowsAsync<Exceptions.NotFoundException>(async () => await _empPositionsService
                .DeletePosition(NonExistingEmpId));
        }

        [Fact]
        public async Task DeletePosition_PassTheIDThatBelongsToTheManager_ReturnPermissionException()
        {
            //Act & Assert
            await Assert.ThrowsAsync<Exceptions.PermissionException>(async () => await _empPositionsService
                .DeletePosition(ExistingEmpIdManager));
        }

        [Fact]
        public async Task DeletePosition_ExistingId_ReturnDeletedEmpPosition()
        {
            //Act
            await _empPositionsService.DeletePosition(ExistingEmpIdMachinist);

            //Assert
            _permissionRepositoryMock.Verify(repo => repo.ReturnPermissionInfoById(ExistingEmpIdManager, _empPositionEntityName), Times.Once);
            _empPositionRepositoryMock.Verify(repo => repo.GetPositionById(ExistingEmpIdMachinist), Times.Once);
        }

        [Fact]
        public async Task DeletePosition_AccessNotAllowed_ReturnPermissionException()
        {
            //Arrange
            _tokenHelperMock.Setup(token => token.CreateAuthViewFromToken()).Returns(_authViewWithNotAccessPermissions);

            //Act & Assert
            await Assert.ThrowsAsync<Exceptions.PermissionException>(async () => await _empPositionsService
                .DeletePosition(ExistingEmpIdMachinist));
        }
    }
}
