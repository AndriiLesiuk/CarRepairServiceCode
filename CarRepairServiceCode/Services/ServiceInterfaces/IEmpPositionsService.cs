using CarRepairServiceCode.RequestModels.Authorization;
using CarRepairServiceCode.RequestModels.EmpPosition;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRepairServiceCode.Services.ServiceInterfaces
{
    public interface IEmpPositionsService
    {
        Task<EmpPositionView> AddPosition(EmpPositionRequest empPositionRequest);
        Task<IEnumerable<EmpPositionView>> GetPositions(EmpPositionQuery empPositionQuery);
        Task<EmpPositionView> GetPositionById(int id);
        Task<EmpPositionView> UpdatePosition(int id, EmpPositionRequest empPositionRequest);
        Task DeletePosition(int id);
    }
}
