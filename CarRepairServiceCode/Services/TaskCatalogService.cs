using AutoMapper;
using CarRepairServiceCode.Helper;
using CarRepairServiceCode.Helper.HelperInterfaces;
using CarRepairServiceCode.Repository.Interfaces;
using CarRepairServiceCode.Repository.Models;
using CarRepairServiceCode.RequestModels.TaskCatalog;
using CarRepairServiceCode.Services.ServiceInterfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRepairServiceCode.Services
{
    public class TaskCatalogService : BaseService<ILogger<TaskCatalogService>>, ITaskCatalogService
    {
        private readonly ITaskCatalogRepository _taskCatalogRepository;
        private readonly IMapper _mapper;
        private readonly ITokenHelper _tokenHelper;
        private readonly DbEntitiesName _taskCatalogEntityName = DbEntitiesName.TaskCatalog;

        public TaskCatalogService(ITaskCatalogRepository taskCatalogRepository, IMapper mapper, ITokenHelper tokenHelper, ILogger<TaskCatalogService> logger, IPermissionRepository permissionRepository) : base(logger, permissionRepository)
        {
            _taskCatalogRepository = taskCatalogRepository;
            _mapper = mapper;
            _tokenHelper = tokenHelper;
        }

        public async Task<TaskCatalogView> AddTask(TaskCatalogRequest taskRequest)
        {
            var authView = _tokenHelper.CreateAuthViewFromToken();
            CheckPermissionInfo(authView.PositionId, ActionName.CreateEntity, _taskCatalogEntityName);

            TaskCatalog taskFromRequest = _mapper.Map<TaskCatalog>(taskRequest);
            taskFromRequest.CreatedById = authView.EmployeeId;
            taskFromRequest.CreatedDate = DateTime.UtcNow;
            TaskCatalog task = await _taskCatalogRepository.AddTask(taskFromRequest);
            TaskCatalogView taskView = _mapper.Map<TaskCatalogView>(task);

            return taskView;
        }

        public async Task<IEnumerable<TaskCatalogView>> GetTasks(TaskCatalogQuery taskQuery)
        {
            var authView = _tokenHelper.CreateAuthViewFromToken();
            CheckPermissionInfo(authView.PositionId, ActionName.ReadEntity, _taskCatalogEntityName);

            IEnumerable<TaskCatalog> list = await _taskCatalogRepository.GetTasks(taskQuery);
            if (list == null)
            {
                return new List<TaskCatalogView>();
            }
            IEnumerable<TaskCatalogView> listView = list.Select(x => _mapper.Map<TaskCatalogView>(x));

            return listView;
        }

        public async Task<TaskCatalogView> GetTaskById(int id)
        {
            var authView = _tokenHelper.CreateAuthViewFromToken();
            CheckPermissionInfo(authView.PositionId, ActionName.ReadEntity, _taskCatalogEntityName);

            TaskCatalog task = await GetTaskByIdFromRepo(id);

            TaskCatalogView taskView = _mapper.Map<TaskCatalogView>(task);

            return taskView;
        }

        public async Task<TaskCatalogView> UpdateTask(int id, TaskCatalogRequest taskRequest)
        {
            var authView = _tokenHelper.CreateAuthViewFromToken();
            CheckPermissionInfo(authView.PositionId, ActionName.UpdateEntity, _taskCatalogEntityName);

            TaskCatalog task = await GetTaskByIdFromRepo(id);

            task = _mapper.Map(taskRequest, task);
            task.ModifiedDate = DateTime.UtcNow;
            await _taskCatalogRepository.UpdateTask(task);
            TaskCatalogView taskView = _mapper.Map<TaskCatalogView>(task);

            return taskView;
        }

        public async Task DeleteTask(int id)
        {
            var authView = _tokenHelper.CreateAuthViewFromToken();
            CheckPermissionInfo(authView.PositionId, ActionName.DeleteEntity, _taskCatalogEntityName);

            TaskCatalog task = await GetTaskByIdFromRepo(id);

            await _taskCatalogRepository.DeleteTask(task);
        }

        private async Task<TaskCatalog> GetTaskByIdFromRepo(int id)
        {
            TaskCatalog taskCatalog = await _taskCatalogRepository.GetTaskById(id);
            if (taskCatalog == null)
                NotFoundExceptionRunner(id);

            return taskCatalog;
        }
    }
}
