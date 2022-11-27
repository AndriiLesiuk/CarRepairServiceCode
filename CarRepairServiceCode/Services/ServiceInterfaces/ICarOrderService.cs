using CarRepairServiceCode.RequestModels.CarOrder;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarRepairServiceCode.RequestModels.Authorization;

namespace CarRepairServiceCode.Services.ServiceInterfaces
{
    public interface ICarOrderService
    {
        Task<CarOrderView> AddCarOrder(CarOrderRequest carOrderRequest);
        Task<IEnumerable<CarOrderView>> GetCarOrders(CarOrderQuery carOrderQuery);
        Task<CarOrderView> GetCarOrderById(int id);
        Task<CarOrderView> UpdateCarOrder(int id, CarOrderRequest carOrderRequest);
        Task DeleteCarOrder(int id);
    }
}
