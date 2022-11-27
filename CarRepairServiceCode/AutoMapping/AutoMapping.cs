using AutoMapper;
using CarRepairServiceCode.Repository.Models;
using CarRepairServiceCode.RequestModels.CarOrder;
using CarRepairServiceCode.RequestModels.CarOrderDetail;
using CarRepairServiceCode.RequestModels.EmpPosition;
using CarRepairServiceCode.RequestModels.TaskCatalog;

namespace CarRepairServiceCode.AutoMapping
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<EmpPosition, EmpPositionView>();
            CreateMap<EmpPositionRequest, EmpPosition>();
            CreateMap<CarOrder, CarOrderView>();
            CreateMap<CarOrderRequest, CarOrder>();
            CreateMap<CarOrderDetail, CarOrderDetailView>();
            CreateMap<CarOrderDetailRequest, CarOrderDetail>();
            CreateMap<TaskCatalog, TaskCatalogView>();
            CreateMap<TaskCatalogRequest, TaskCatalog>();
        }
    }
}
