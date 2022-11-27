using CarRepairServiceCode.Helper;
using CarRepairServiceCode.RequestModels.CarOrder;
using CarRepairServiceCode.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace CarRepairServiceCode.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CarOrderController : ControllerBase
    {
        private readonly ICarOrderService _carOrderService;
        private readonly ILogger<CarOrderController> _logger;

        public CarOrderController(ICarOrderService carOrderService, ILogger<CarOrderController> logger)
        {
            _carOrderService = carOrderService;
            _logger = logger;
        }

        [SwaggerOperation(Summary = "Add new Car_Order", Description = "Return added order.")]
        [SwaggerResponse(201, "Order successfully added!", typeof(CarOrderView))]
        [SwaggerResponse(401, Messages.SwaggerUnauthorizedMessage)]
        [HttpPost("AddCarOrder")]
        public async Task<IActionResult> AddCarOrder([FromBody] CarOrderRequest carOrderRequest)
        {
            var orderView = await _carOrderService.AddCarOrder(carOrderRequest);

            return Created(string.Format(Messages.SuccessfullyAdded, Entities.CarOrder.GetEnumDescription(), orderView.OrderId), orderView);
        }

        [SwaggerOperation(Summary = "Filter Car_Order", Description = "Return result of filtered car orders.")]
        [SwaggerResponse(200, "Car order list was returned.", typeof(CarOrderView))]
        [SwaggerResponse(401, Messages.SwaggerUnauthorizedMessage)]
        [HttpGet("GetCarOrders")]
        public async Task<IActionResult> GetCarOrders([FromQuery] CarOrderQuery carOrderQuery)
        {
            var orderView = await _carOrderService.GetCarOrders(carOrderQuery);

            return Ok(orderView);
        }

        [SwaggerOperation(Summary = "Get Car_Order By Id", Description = "Return car order by id.")]
        [SwaggerResponse(200, "Car order was returned.", typeof(CarOrderView))]
        [SwaggerResponse(401, Messages.SwaggerUnauthorizedMessage)]
        [SwaggerResponse(404, "Order not found in the system!")]
        [HttpGet("GetCarOrderById/{id}")]
        public async Task<IActionResult> GetCarOrderById(int id)
        {
            var orderView = await _carOrderService.GetCarOrderById(id);

            return Ok(orderView);
        }

        [SwaggerOperation(Summary = "Update Car_Order", Description = "Update order by id.")]
        [SwaggerResponse(200, "Order was updated.", typeof(CarOrderView))]
        [SwaggerResponse(401, Messages.SwaggerUnauthorizedMessage)]
        [SwaggerResponse(403, Messages.SwaggerAccessDenied)]
        [SwaggerResponse(404, "Order not found in the system!")]
        [HttpPut("UpdateCarOrder/{id}")]
        public async Task<IActionResult> UpdateCarOrder(int id, [FromBody] CarOrderRequest carOrderRequest)
        {
            var orderView = await _carOrderService.UpdateCarOrder(id, carOrderRequest);

            return Ok(orderView);
        }

        [SwaggerOperation(Summary = "Delete Car_Order", Description = "Delete order by id.")]
        [SwaggerResponse(204, "Order was deleted.")]
        [SwaggerResponse(401, Messages.SwaggerUnauthorizedMessage)]
        [SwaggerResponse(403, Messages.SwaggerAccessDenied)]
        [SwaggerResponse(404, "Order not found in the system!")]
        [HttpDelete("DeleteCarOrder/{id}")]
        public async Task<IActionResult> DeleteCarOrder(int id)
        {
            await _carOrderService.DeleteCarOrder(id);

            return NoContent();
        }
    }
}
