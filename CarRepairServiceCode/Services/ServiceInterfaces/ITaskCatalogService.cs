using CarRepairServiceCode.RequestModels.Authorization;
using CarRepairServiceCode.RequestModels.TaskCatalog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRepairServiceCode.Services.ServiceInterfaces
{
    public interface ITaskCatalogService
    {
        Task<TaskCatalogView> AddTask(TaskCatalogRequest taskRequest);
        Task<IEnumerable<TaskCatalogView>> GetTasks(TaskCatalogQuery taskQuery);
        Task<TaskCatalogView> GetTaskById(int id);
        Task<TaskCatalogView> UpdateTask(int id, TaskCatalogRequest taskRequest);
        Task DeleteTask(int id);
    }
}