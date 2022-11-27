using CarRepairServiceCode.Helper.HelperInterfaces;
using CarRepairServiceCode.RequestModels.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace CarRepairServiceCode.Helper
{
    public class TokenHelper : ITokenHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public AuthView CreateAuthViewFromToken()
        {
            string token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            string editedToken = token.Replace("Bearer ", "");
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(editedToken);
            var employeeId = jwtToken.Claims.FirstOrDefault(claim => claim.Type == Constants.EmployeeId)?.Value;
            var firstName = jwtToken.Claims.FirstOrDefault(claim => claim.Type == Constants.FirstName)?.Value;
            var lastName = jwtToken.Claims.FirstOrDefault(claim => claim.Type == Constants.LastName)?.Value;
            var positionId = jwtToken.Claims.FirstOrDefault(claim => claim.Type == Constants.PositionId)?.Value;
            var isActive = jwtToken.Claims.FirstOrDefault(claim => claim.Type == Constants.IsActive)?.Value;
            var expirationTime = jwtToken.Claims.FirstOrDefault(claim => claim.Type == Constants.ExpirationTime)?.Value;
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expirationTime));

            AuthView authView = new AuthView
            {
                EmployeeId = Int32.Parse(employeeId),
                FirstName = firstName,
                LastName = lastName,
                PositionId = Int32.Parse(positionId),
                IsActive = bool.Parse(isActive),
                Token = editedToken,
                Expiration = dateTimeOffset.LocalDateTime
            };

            return authView;
        }
    }
}
