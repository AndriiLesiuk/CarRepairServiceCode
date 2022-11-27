using CarRepairServiceCode.Repository.Models;
using CarRepairServiceCode.RequestModels.TaskCatalog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRepairServiceCode.Repository.Interfaces
{
    public interface ITaskCatalogRepository
    {
        Task<TaskCatalog> AddTask(TaskCatalog task);
        Task<IEnumerable<TaskCatalog>> GetTasks(TaskCatalogQuery taskQuery);
        Task<TaskCatalog> GetTaskById(int id);
        Task<TaskCatalog> UpdateTask(TaskCatalog task);
        Task DeleteTask(TaskCatalog task);
    }
}