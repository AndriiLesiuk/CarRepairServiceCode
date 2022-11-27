using CarRepairServiceCode.Repository.Contexts;
using CarRepairServiceCode.Repository.Interfaces;
using CarRepairServiceCode.Repository.Models;
using CarRepairServiceCode.RequestModels.TaskCatalog;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRepairServiceCode.Repository.Repositories
{
    public class TaskCatalogRepository : BaseRepository<CarRepairServiceDB_Context>, ITaskCatalogRepository
    {
        public TaskCatalogRepository(CarRepairServiceDB_Context context) : base(context)
        {
        }

        public async Task<TaskCatalog> AddTask(TaskCatalog task)
        {
            _context.TaskCatalogs.Add(task);
            await _context.SaveChangesAsync();

            return task;
        }

        public async Task<IEnumerable<TaskCatalog>> GetTasks(TaskCatalogQuery taskQuery)
        {
            var result = _context.TaskCatalogs.AsQueryable();

            if (taskQuery.TaskName != null)
                result = result.Where(x => x.TaskName.ToLower().Contains(taskQuery.TaskName.ToLower()));

            if (taskQuery.TaskDescription != null)
                result = result.Where(x => x.TaskDescription.ToLower().Contains(taskQuery.TaskDescription.ToLower()));

            if (taskQuery.TaskPrice != null)
                result = result.Where(x => x.TaskPrice == taskQuery.TaskPrice);

            return await result.ToListAsync();
        }

        public async Task<TaskCatalog> GetTaskById(int id)
        {
            var result = await _context.TaskCatalogs.FirstOrDefaultAsync(p => p.TaskId == id);
            return result;
        }

        public async Task<TaskCatalog> UpdateTask(TaskCatalog task)
        {
            _context.Update(task);
            await _context.SaveChangesAsync();

            return task;
        }

        public async Task DeleteTask(TaskCatalog task)
        {
            _context.TaskCatalogs.Remove(task);
            await _context.SaveChangesAsync();
        }
    }
}