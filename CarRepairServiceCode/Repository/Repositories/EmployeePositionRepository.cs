using System;
using CarRepairServiceCode.Repository.Contexts;
using CarRepairServiceCode.Repository.Interfaces;
using CarRepairServiceCode.Repository.Models;
using CarRepairServiceCode.RequestModels.EmpPosition;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRepairServiceCode.Repository.Repositories
{
    public class EmployeePositionRepository : BaseRepository<CarRepairServiceDB_Context>, IEmpPositionRepository
    {
        public EmployeePositionRepository(CarRepairServiceDB_Context context) : base(context)
        {
        }

        public async Task<EmpPosition> AddPosition(EmpPosition position)
        {
            _context.EmpPositions.Add(position);
            await _context.SaveChangesAsync();

            return position;
        }

        public async Task<IEnumerable<EmpPosition>> GetPositions(EmpPositionQuery empPositionQuery)
        {
            var result = _context.EmpPositions.AsQueryable();

            if (empPositionQuery.PositionName != null)
                result = result.Where(x => x.PositionName.ToLower().Contains(empPositionQuery.PositionName.ToLower()));

            return await result.ToListAsync();
        }

        public async Task<EmpPosition> GetPositionById(int id)
        {
            var result = await _context.EmpPositions.FirstOrDefaultAsync(p => p.PositionId == id);
            return result;
        }

        public async Task<EmpPosition> UpdatePosition(EmpPosition empPosition)
        {
            _context.Update(empPosition);
            await _context.SaveChangesAsync();

            return empPosition;
        }

        public async Task DeletePosition(EmpPosition empPosition)
        {
            _context.EmpPositions.Remove(empPosition);
            await _context.SaveChangesAsync();
        }
    }
}
