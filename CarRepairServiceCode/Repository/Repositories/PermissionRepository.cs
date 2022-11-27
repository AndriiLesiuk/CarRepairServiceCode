using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CarRepairServiceCode.Helper;
using CarRepairServiceCode.Repository.Contexts;
using CarRepairServiceCode.Repository.Interfaces;
using CarRepairServiceCode.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRepairServiceCode.Repository.Repositories
{
    public class PermissionRepository : BaseRepository<CarRepairServiceDB_Context>, IPermissionRepository
    {
        public PermissionRepository(CarRepairServiceDB_Context context) : base(context)
        {
        }

        public async Task<Permissions> ReturnPermissionInfoById(int positionId, string entityName)
        {
            var result = await _context.Permissions.FirstOrDefaultAsync(x => x.PositionId == positionId && x.EntityForAction == entityName);
            return result;
        }
    }
}
