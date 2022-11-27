using AutoMapper;
using CarRepairServiceCode.Helper;
using CarRepairServiceCode.Helper.HelperInterfaces;
using CarRepairServiceCode.RabbitMQServices.Interfaces;
using CarRepairServiceCode.RabbitMQServices.Models;
using CarRepairServiceCode.Repository.Interfaces;
using CarRepairServiceCode.Repository.Models;
using CarRepairServiceCode.RequestModels.CarOrder;
using CarRepairServiceCode.Services.ServiceInterfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRepairServiceCode.Services
{
    public class CarOrderService : BaseService<ILogger<CarOrderService>>, ICarOrderService
    {
        private readonly ICarOrderRepository _carOrderRepository;
        private readonly IMapper _mapper;
        private readonly ITokenHelper _tokenHelper;
        private readonly IAccountingInfoSender _carOrderAddedSender;
        private readonly DbEntitiesName _carOrderEntityName = DbEntitiesName.CarOrder;

        public CarOrderService(ICarOrderRepository carOrderRepository, IMapper mapper, ITokenHelper tokenHelper, ILogger<CarOrderService> logger, IPermissionRepository permissionRepository, IAccountingInfoSender carOrderAddedSender) : base(logger, permissionRepository)
        {
            _carOrderRepository = carOrderRepository;
            _mapper = mapper;
            _tokenHelper = tokenHelper;
            _carOrderAddedSender = carOrderAddedSender;
        }

        public async Task<CarOrderView> AddCarOrder(CarOrderRequest carOrderRequest)
        {
            var authView = _tokenHelper.CreateAuthViewFromToken();
            CheckPermissionInfo(authView.PositionId, ActionName.CreateEntity, _carOrderEntityName);

            CarOrder orderFromRequest = _mapper.Map<CarOrder>(carOrderRequest);
            orderFromRequest.CreatedById = authView.EmployeeId;
            orderFromRequest.CreatedDate = DateTime.UtcNow;
            CarOrder order = await _carOrderRepository.AddCarOrder(orderFromRequest);

            EntityInfo<CarOrder> orderInfo = new EntityInfo<CarOrder> { Info = order, Action = ActionName.CreateEntity.GetEnumDescription() };
            _carOrderAddedSender.SendEntityInfo(orderInfo);

            CarOrderView view = _mapper.Map<CarOrderView>(order);

            return view;
        }

        public async Task<IEnumerable<CarOrderView>> GetCarOrders(CarOrderQuery carOrderQuery)
        {
            var authView = _tokenHelper.CreateAuthViewFromToken();
            CheckPermissionInfo(authView.PositionId, ActionName.ReadEntity, _carOrderEntityName);

            IEnumerable<CarOrder> list = await _carOrderRepository.GetCarOrders(carOrderQuery);
            if (list == null)
            {
                return new List<CarOrderView>();
            }

            IEnumerable<CarOrderView> view = list.Select(x => _mapper.Map<CarOrderView>(x));

            return view;
        }

        public async Task<CarOrderView> GetCarOrderById(int id)
        {
            var authView = _tokenHelper.CreateAuthViewFromToken();
            CheckPermissionInfo(authView.PositionId, ActionName.ReadEntity, _carOrderEntityName);

            CarOrder order = await GetOrderByIdFromRepo(id);
            CarOrderView view = _mapper.Map<CarOrderView>(order);

            return view;
        }

        public async Task<CarOrderView> UpdateCarOrder(int id, CarOrderRequest carOrderRequest)
        {
            var authView = _tokenHelper.CreateAuthViewFromToken();
            CheckPermissionInfo(authView.PositionId, ActionName.UpdateEntity, _carOrderEntityName);

            CarOrder order = await GetOrderByIdFromRepo(id);

            order = _mapper.Map(carOrderRequest, order);
            order.ModifiedDate = DateTime.UtcNow;
            await _carOrderRepository.UpdateCarOrder(order);

            EntityInfo<CarOrder> orderInfo = new EntityInfo<CarOrder> { Info = order, Action = ActionName.UpdateEntity.GetEnumDescription() };
            _carOrderAddedSender.SendEntityInfo(orderInfo);

            CarOrderView view = _mapper.Map<CarOrderView>(order);

            return view;
        }

        public async Task DeleteCarOrder(int id)
        {
            var authView = _tokenHelper.CreateAuthViewFromToken();
            CheckPermissionInfo(authView.PositionId, ActionName.DeleteEntity, _carOrderEntityName);

            CarOrder order = await GetOrderByIdFromRepo(id);

            await _carOrderRepository.DeleteCarOrder(order);

            EntityInfo<CarOrder> orderInfo = new EntityInfo<CarOrder> { Info = order, Action = ActionName.DeleteEntity.GetEnumDescription() };
            _carOrderAddedSender.SendEntityInfo(orderInfo);
        }

        private async Task<CarOrder> GetOrderByIdFromRepo(int id)
        {
            CarOrder order = await _carOrderRepository.GetCarOrderById(id);
            if (order == null)
                NotFoundExceptionRunner(id);

            return order;
        }
    }
}
