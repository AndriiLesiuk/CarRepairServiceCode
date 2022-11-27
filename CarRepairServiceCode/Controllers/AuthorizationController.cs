using CarRepairServiceCode.Helper;
using CarRepairServiceCode.RequestModels.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;
using IAuthorizationService = CarRepairServiceCode.Services.ServiceInterfaces.IAuthorizationService;

namespace CarRepairServiceCode.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly ILogger<AuthorizationController> _logger;

        public AuthorizationController(IAuthorizationService authorizationService, ILogger<AuthorizationController> logger)
        {
            _authorizationService = authorizationService;
            _logger = logger;
        }

        [AllowAnonymous]
        [SwaggerOperation(Summary = "Get JWT Token", Description = "Return JWT token for Authorization")]
        [SwaggerResponse(200, "Token successfully issued.", typeof(AuthView))]
        [SwaggerResponse(401, "Invalid credentials. Try again.")]
        [HttpPost("GetToken")]
        public async Task<IActionResult> GenerateTokenForEmployee([FromForm] AuthRequest authRequest)
        {
            var resultView = await _authorizationService.GenerateTokenForEmployee(authRequest);
            _logger.LogInformation(string.Format(Messages.TokenSuccessfullyGenerated, resultView.FirstName, resultView.LastName, resultView.EmployeeId));

            return Ok(resultView);
        }

        [SwaggerOperation(Summary = "Refresh JWT Token", Description = "Refresh JWT token for Authorization")]
        [SwaggerResponse(200, "Token successfully refreshed.", typeof(AuthView))]
        [SwaggerResponse(401, Messages.SwaggerUnauthorizedMessage)]
        [HttpGet("RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var response = await _authorizationService.RefreshToken();

            return Ok(response);
        }
    }
}
