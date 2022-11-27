using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRepairServiceCode.Repository.Models;
using CarRepairServiceCode.RequestModels.CarOrder;

namespace CarRepairServiceCode.Repository.Interfaces
{
    public interface ICarOrderRepository
    {
        Task<CarOrder> AddCarOrder(CarOrder carOrder);        
        Task<IEnumerable<CarOrder>> GetCarOrders(CarOrderQuery carOrderQuery);
        Task<CarOrder> GetCarOrderById(int id);
        Task<CarOrder> UpdateCarOrder(CarOrder carOrder);
        Task DeleteCarOrder(CarOrder carOrder);
    }
}
