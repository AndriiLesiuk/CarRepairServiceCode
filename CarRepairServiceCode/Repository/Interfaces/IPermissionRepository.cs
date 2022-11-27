using CarRepairServiceCode.Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRepairServiceCode.Repository.Interfaces
{
    public interface IPermissionRepository
    {
        Task<Permissions> ReturnPermissionInfoById(int positionId, string entityName);
    }
}
