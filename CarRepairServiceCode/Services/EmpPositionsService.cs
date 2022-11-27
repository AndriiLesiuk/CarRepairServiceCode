using AutoMapper;
using CarRepairServiceCode.Helper;
using CarRepairServiceCode.Helper.HelperInterfaces;
using CarRepairServiceCode.Repository.Interfaces;
using CarRepairServiceCode.Repository.Models;
using CarRepairServiceCode.RequestModels.EmpPosition;
using CarRepairServiceCode.Services.ServiceInterfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRepairServiceCode.Services
{
    public class EmpPositionsService : BaseService<ILogger<EmpPositionsService>>, IEmpPositionsService
    {
        private readonly IEmpPositionRepository _empPositionRepository;
        private readonly IMapper _mapper;
        private readonly ITokenHelper _tokenHelper;
        private readonly DbEntitiesName _empPositionEntityName = DbEntitiesName.EmpPosition;

        public EmpPositionsService(IEmpPositionRepository empPositionRepository, IMapper mapper,
            ITokenHelper tokenHelper, ILogger<EmpPositionsService> logger, IPermissionRepository permissionRepository) : base(logger, permissionRepository)
        {
            _empPositionRepository = empPositionRepository;
            _mapper = mapper;
            _tokenHelper = tokenHelper;
        }

        public async Task<EmpPositionView> AddPosition(EmpPositionRequest empPositionRequest)
        {
            var authView = _tokenHelper.CreateAuthViewFromToken();
            CheckPermissionInfo(authView.PositionId, ActionName.CreateEntity, _empPositionEntityName);

            EmpPosition positionFromRequest = _mapper.Map<EmpPosition>(empPositionRequest);
            positionFromRequest.CreatedById = authView.EmployeeId;
            positionFromRequest.CreatedDate = DateTime.UtcNow;
            EmpPosition position = await _empPositionRepository.AddPosition(positionFromRequest);
            EmpPositionView empView = _mapper.Map<EmpPositionView>(position);

            return empView;
        }

        public async Task<IEnumerable<EmpPositionView>> GetPositions(EmpPositionQuery empPositionQuery)
        {
            var authView = _tokenHelper.CreateAuthViewFromToken();
            CheckPermissionInfo(authView.PositionId, ActionName.ReadEntity, _empPositionEntityName);

            IEnumerable<EmpPosition> list = await _empPositionRepository.GetPositions(empPositionQuery);
            if (list == null)
            {
                return new List<EmpPositionView>();
            }

            IEnumerable<EmpPositionView> listView = list.Select(x => _mapper.Map<EmpPositionView>(x));

            return listView;
        }

        public async Task<EmpPositionView> GetPositionById(int id)
        {
            var authView = _tokenHelper.CreateAuthViewFromToken();
            CheckPermissionInfo(authView.PositionId, ActionName.ReadEntity, _empPositionEntityName);

            EmpPosition position = await GetPositionByIdFromRepo(id);

            EmpPositionView empView = _mapper.Map<EmpPositionView>(position);

            return empView;
        }

        public async Task<EmpPositionView> UpdatePosition(int id, EmpPositionRequest empPositionRequest)
        {
            var authView = _tokenHelper.CreateAuthViewFromToken();
            CheckPermissionInfo(authView.PositionId, ActionName.UpdateEntity, _empPositionEntityName);

            EmpPosition position = await GetPositionByIdFromRepo(id);

            if (position.PositionId == (int)ServicePositions.Manager)
                PermissionErrorRunner(id);

            position = _mapper.Map(empPositionRequest, position);
            position.ModifiedDate = DateTime.UtcNow;
            await _empPositionRepository.UpdatePosition(position);
            EmpPositionView empView = _mapper.Map<EmpPositionView>(position);

            return empView;
        }

        public async Task DeletePosition(int id)
        {
            var authView = _tokenHelper.CreateAuthViewFromToken();
            CheckPermissionInfo(authView.PositionId, ActionName.DeleteEntity, _empPositionEntityName);

            EmpPosition position = await GetPositionByIdFromRepo(id);

            if (position.PositionId == (int)ServicePositions.Manager)
                PermissionErrorRunner(id);

            await _empPositionRepository.DeletePosition(position);
        }

        private async Task<EmpPosition> GetPositionByIdFromRepo(int id)
        {
            EmpPosition position = await _empPositionRepository.GetPositionById(id);
            if (position == null)
                NotFoundExceptionRunner(id);

            return position;
        }
    }
}
