using CarRepairServiceCode.RequestModels.Authorization;

namespace CarRepairServiceCode.Helper.HelperInterfaces
{
    public interface ITokenHelper
    {
        AuthView CreateAuthViewFromToken();
    }
}
