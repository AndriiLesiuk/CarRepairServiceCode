using CarRepairServiceCode.Helper;
using CarRepairServiceCode.RequestModels.TaskCatalog;
using CarRepairServiceCode.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace CarRepairServiceCode.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskCatalogController : ControllerBase
    {
        private readonly ITaskCatalogService _taskCatalogService;
        private readonly ILogger<TaskCatalogController> _logger;

        public TaskCatalogController(ITaskCatalogService taskCatalogService, ILogger<TaskCatalogController> logger)
        {
            _taskCatalogService = taskCatalogService;
            _logger = logger;
        }

        [SwaggerOperation(Summary = "Add new Task_Catalog", Description = "Return added task.")]
        [SwaggerResponse(201, "Task successfully added!", typeof(TaskCatalogView))]
        [SwaggerResponse(401, Messages.SwaggerUnauthorizedMessage)]
        [HttpPost("AddTask")]
        public async Task<IActionResult> AddTask([FromBody] TaskCatalogRequest taskCatalogRequest)
        {
            var taskView = await _taskCatalogService.AddTask(taskCatalogRequest);

            return Created(string.Format(Messages.SuccessfullyAdded, Entities.Task.GetEnumDescription(), taskCatalogRequest.TaskName), taskView);
        }

        [SwaggerOperation(Summary = "Filter Task_Catalogs", Description = "Return result of filtered tasks.")]
        [SwaggerResponse(200, "Task list was returned.", typeof(TaskCatalogView))]
        [SwaggerResponse(401, Messages.SwaggerUnauthorizedMessage)]
        [HttpGet("GetTasks")]
        public async Task<IActionResult> GetTasks([FromQuery] TaskCatalogQuery taskCatalogQuery)
        {
            var taskView = await _taskCatalogService.GetTasks(taskCatalogQuery);

            return Ok(taskView);
        }

        [SwaggerOperation(Summary = "Get Task_Catalog By Id", Description = "Return task by id.")]
        [SwaggerResponse(200, "Task was returned.", typeof(TaskCatalogView))]
        [SwaggerResponse(401, Messages.SwaggerUnauthorizedMessage)]
        [SwaggerResponse(404, "Task not found in the system!")]
        [HttpGet("GetTaskById/{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var taskView = await _taskCatalogService.GetTaskById(id);

            return Ok(taskView);
        }

        [SwaggerOperation(Summary = "Update Task_Catalog", Description = "Update task by id.")]
        [SwaggerResponse(200, "Task was updated.", typeof(TaskCatalogView))]
        [SwaggerResponse(401, Messages.SwaggerUnauthorizedMessage)]
        [SwaggerResponse(403, Messages.SwaggerAccessDenied)]
        [SwaggerResponse(404, "Task not found in the system!")]
        [HttpPut("UpdateTask/{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskCatalogRequest taskCatalogRequest)
        {
            var taskView = await _taskCatalogService.UpdateTask(id, taskCatalogRequest);

            return Ok(taskView);
        }

        [SwaggerOperation(Summary = "Delete Task_Catalog", Description = "Delete task by id.")]
        [SwaggerResponse(204, "Task was deleted.")]
        [SwaggerResponse(401, Messages.SwaggerUnauthorizedMessage)]
        [SwaggerResponse(403, Messages.SwaggerAccessDenied)]
        [SwaggerResponse(404, "Task not found in the system!")]
        [HttpDelete("DeleteTask/{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            await _taskCatalogService.DeleteTask(id);

            return NoContent();
        }
    }
}
