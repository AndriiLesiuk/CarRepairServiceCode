using CarRepairServiceCode.RequestModels.EmpPosition;
using CarRepairServiceCode.Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarRepairServiceCode.Repository.Interfaces
{
    public interface IEmpPositionRepository
    {
        Task<EmpPosition> AddPosition(EmpPosition position);        
        Task<IEnumerable<EmpPosition>> GetPositions(EmpPositionQuery empPositionQuery);
        Task<EmpPosition> GetPositionById(int id);
        Task<EmpPosition> UpdatePosition(EmpPosition empPosition);
        Task DeletePosition(EmpPosition empPosition);
    }
}
