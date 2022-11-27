using CarRepairServiceCode.RequestModels.Authorization;
using System.Threading.Tasks;

namespace CarRepairServiceCode.Services.ServiceInterfaces
{
    public interface IAuthorizationService
    {
        Task<AuthView> GenerateTokenForEmployee(AuthRequest empPositionRequest);
        Task<AuthView> RefreshToken();
    }
}
