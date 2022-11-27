using CarRepairServiceCode.Repository.Contexts;
using CarRepairServiceCode.Repository.Interfaces;
using CarRepairServiceCode.Repository.Models;
using CarRepairServiceCode.RequestModels.CarOrder;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRepairServiceCode.Repository.Repositories
{
    public class CarOrderRepository : BaseRepository<CarRepairServiceDB_Context>, ICarOrderRepository
    {
        public CarOrderRepository(CarRepairServiceDB_Context context) : base(context)
        {
        }

        public async Task<CarOrder> AddCarOrder(CarOrder carOrder)
        {
            _context.CarOrders.Add(carOrder);
            await _context.SaveChangesAsync();

            return carOrder;
        }

        public async Task<IEnumerable<CarOrder>> GetCarOrders(CarOrderQuery carOrderQuery)
        {
            var result = _context.CarOrders.Include(x => x.CarOrderDetails).AsQueryable();

            if (carOrderQuery.CarId != null)
                result = result.Where(x => x.CarId == carOrderQuery.CarId);

            if (carOrderQuery.OrderDate != null)
                result = result.Where(x => x.OrderDate == carOrderQuery.OrderDate);

            if (carOrderQuery.OrderAmount != null)
                result = result.Where(x => x.OrderAmount == carOrderQuery.OrderAmount);

            if (carOrderQuery.OrderComments != null)
                result = result.Where(x => x.OrderComments.ToLower().Contains(carOrderQuery.OrderComments.ToLower()));


            return await result.ToListAsync();
        }

        public async Task<CarOrder> GetCarOrderById(int id)
        {
            CarOrder result = await _context.CarOrders.Include(x => x.CarOrderDetails).FirstOrDefaultAsync(p => p.OrderId == id);
            return result;
        }

        public async Task<CarOrder> UpdateCarOrder(CarOrder carOrder)
        {
            _context.Update(carOrder);
            await _context.SaveChangesAsync();

            return carOrder;
        }

        public async Task DeleteCarOrder(CarOrder carOrder)
        {
            _context.CarOrders.Remove(carOrder);
            await _context.SaveChangesAsync();
        }
    }
}
