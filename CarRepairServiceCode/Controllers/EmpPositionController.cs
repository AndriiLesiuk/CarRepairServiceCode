using CarRepairServiceCode.Helper;
using CarRepairServiceCode.RequestModels.EmpPosition;
using CarRepairServiceCode.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CarRepairServiceCode.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmpPositionController : ControllerBase
    {
        private readonly IEmpPositionsService _empPositionsService;
        private readonly ILogger<EmpPositionController> _logger;

        public EmpPositionController(IEmpPositionsService empPositionsService, ILogger<EmpPositionController> logger)
        {
            _empPositionsService = empPositionsService;
            _logger = logger;
        }

        [SwaggerOperation(Summary = "Add new Emp_Position", Description = "Return added position.")]
        [SwaggerResponse(201, "Position successfully added!", typeof(EmpPositionView))]
        [SwaggerResponse(401, Messages.SwaggerUnauthorizedMessage)]
        [HttpPost("AddPosition")]
        public async Task<IActionResult> AddPosition([FromBody] EmpPositionRequest empPositionRequest)
        {
            var positionView = await _empPositionsService.AddPosition(empPositionRequest);

            return Created(string.Format(Messages.SuccessfullyAdded, Entities.Position.GetEnumDescription(), empPositionRequest.PositionName), positionView);
        }

        [SwaggerOperation(Summary = "Filter Emp_Position", Description = "Return result of filtered positions.")]
        [SwaggerResponse(200, "Positions list was returned.", typeof(EmpPositionView))]
        [SwaggerResponse(401, Messages.SwaggerUnauthorizedMessage)]
        [HttpGet("GetPositions")]
        public async Task<IActionResult> GetPositions([FromQuery] EmpPositionQuery empPositionQuery)
        {
            var positionView = await _empPositionsService.GetPositions(empPositionQuery);

            return Ok(positionView);
        }

        [SwaggerOperation(Summary = "Get Emp_Position By Id", Description = "Return position by id.")]
        [SwaggerResponse(200, "Position was returned.", typeof(EmpPositionView))]
        [SwaggerResponse(401, Messages.SwaggerUnauthorizedMessage)]
        [SwaggerResponse(404, "Position not found in the system!")]
        [HttpGet("GetPositionById/{id}")]
        public async Task<IActionResult> GetPositionById(int id)
        {
            var positionView = await _empPositionsService.GetPositionById(id);

            return Ok(positionView);
        }

        [SwaggerOperation(Summary = "Update Emp_Position", Description = "Update position by id.")]
        [SwaggerResponse(200, "Position was updated.", typeof(EmpPositionView))]
        [SwaggerResponse(401, Messages.SwaggerUnauthorizedMessage)]
        [SwaggerResponse(403, Messages.SwaggerAccessDenied)]
        [SwaggerResponse(404, "Position not found in the system!")]
        [HttpPut("UpdatePosition/{id}")]
        public async Task<IActionResult> UpdatePosition(int id, [FromBody] EmpPositionRequest empPositionRequest)
        {
            var positionView = await _empPositionsService.UpdatePosition(id, empPositionRequest);

            return Ok(positionView);
        }

        [SwaggerOperation(Summary = "Delete Emp_Position", Description = "Delete position by id.")]
        [SwaggerResponse(204, "Position was deleted.")]
        [SwaggerResponse(401, Messages.SwaggerUnauthorizedMessage)]
        [SwaggerResponse(403, Messages.SwaggerAccessDenied)]
        [SwaggerResponse(404, "Position not found in the system!")]
        [HttpDelete("DeletePosition/{id}")]
        public async Task<IActionResult> DeletePosition(int id)
        {
            await _empPositionsService.DeletePosition(id);

            return NoContent();
        }
    }
}
