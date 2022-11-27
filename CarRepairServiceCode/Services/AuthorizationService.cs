using CarRepairServiceCode.Helper;
using CarRepairServiceCode.Helper.HelperInterfaces;
using CarRepairServiceCode.Repository.Interfaces;
using CarRepairServiceCode.Repository.Models;
using CarRepairServiceCode.RequestModels.Authorization;
using CarRepairServiceCode.Services.ServiceInterfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CarRepairServiceCode.Services
{
    public class AuthorizationService : BaseService<ILogger<AuthorizationService>>, IAuthorizationService
    {
        private readonly IAuthorizationRepository _authorizationRepository;
        private readonly AuthOptions _authOptions;
        private readonly ITokenHelper _tokenHelper;

        public AuthorizationService(IAuthorizationRepository authorizationRepository, IOptions<AuthOptions> authOptionsAccessor, ILogger<AuthorizationService> logger, ITokenHelper tokenHelper, IPermissionRepository permissionRepository) : base(logger, permissionRepository)
        {
            _authorizationRepository = authorizationRepository;
            _tokenHelper = tokenHelper;
            _authOptions = authOptionsAccessor.Value;
        }

        public async Task<AuthView> GenerateTokenForEmployee(AuthRequest empPositionRequest)
        {
            var password = SecurePasswordHasherHelper.Hash(empPositionRequest.EmpPassword);
            Employee emp = await _authorizationRepository.GetEmployeeByCredentials(empPositionRequest.EmpLogin, password);

            if (emp == null)
                UnauthorizedExceptionRunner(empPositionRequest.EmpLogin);

            var token = GenerateToken(emp.EmpLogin, Guid.NewGuid().ToString(), emp.EmployeeId.ToString(), emp.FirstName, emp.LastName, emp.PositionId.ToString(), emp.IsActive.ToString());

            AuthView authView = new AuthView
            {
                EmployeeId = emp.EmployeeId,
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                PositionId = emp.PositionId,
                IsActive = emp.IsActive,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            };

            return authView;
        }

        public async Task<AuthView> RefreshToken()
        {
            var authViewFromRequest = _tokenHelper.CreateAuthViewFromToken();
            var oldToken = authViewFromRequest.Token;
            var handler = new JwtSecurityTokenHandler();
            var sub = handler.ReadJwtToken(oldToken).Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub)?.Value;
            var jti = handler.ReadJwtToken(oldToken).Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Jti)?.Value;
            var empId = handler.ReadJwtToken(oldToken).Claims.FirstOrDefault(claim => claim.Type == Constants.EmployeeId)?.Value;
            var fName = handler.ReadJwtToken(oldToken).Claims.FirstOrDefault(claim => claim.Type == Constants.FirstName)?.Value;
            var lName = handler.ReadJwtToken(oldToken).Claims.FirstOrDefault(claim => claim.Type == Constants.LastName)?.Value;
            var posId = handler.ReadJwtToken(oldToken).Claims.FirstOrDefault(claim => claim.Type == Constants.PositionId)?.Value;
            var isAct = handler.ReadJwtToken(oldToken).Claims.FirstOrDefault(claim => claim.Type == Constants.IsActive)?.Value;

            var tokenForReturn = GenerateToken(sub, jti, empId, fName, lName, posId, isAct);

            AuthView authView = new AuthView
            {
                EmployeeId = Int32.Parse(empId),
                FirstName = fName,
                LastName = lName,
                PositionId = Int32.Parse(posId),
                IsActive = bool.Parse(isAct),
                Token = new JwtSecurityTokenHandler().WriteToken(tokenForReturn),
                Expiration = tokenForReturn.ValidTo
            };

            return authView;
        }

        private JwtSecurityToken GenerateToken(string sub, string jti, string employeeId, string firstName, string lastName, string positionId, string isActive)
        {
            string role = Int32.Parse(positionId) == (int)ServicePositions.Manager ? "Manager" : "User";

            var authClaims = new[]
            {
                    new Claim(JwtRegisteredClaimNames.Sub, sub),
                    new Claim(JwtRegisteredClaimNames.Jti, jti),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, role),
                    new Claim(Constants.EmployeeId, employeeId),
                    new Claim(Constants.FirstName, firstName),
                    new Claim(Constants.LastName, lastName),
                    new Claim(Constants.PositionId, positionId),
                    new Claim(Constants.IsActive, isActive)
            };

            var token = new JwtSecurityToken(
                    issuer: _authOptions.Issuer,
                    audience: _authOptions.Audience,
                    expires: DateTime.UtcNow.AddMinutes(_authOptions.ExpiresInMinutes),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authOptions.SecureKey)),
                        SecurityAlgorithms.HmacSha256Signature)
                    );

            return token;
        }
    }
}
