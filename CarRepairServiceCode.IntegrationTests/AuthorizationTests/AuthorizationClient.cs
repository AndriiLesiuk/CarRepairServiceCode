using CarRepairServiceCode.IntegrationTests.BaseEntities;
using CarRepairServiceCode.RequestModels.Authorization;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CarRepairServiceCode.IntegrationTests.AuthorizationTests
{
    public class AuthorizationClient : BaseClient
    {
        private const string GetTokenPath = @"/api/Authorization/GetToken/";
        private const string RefreshTokenPath = @"/api/Authorization/RefreshToken/";

        public async Task<HttpResponseDTO<AuthView>> GenerateTokenForEmployee(HttpClient client, AuthRequest authRequest)
        {
            var authCredentials = AuthRequestConverter(authRequest);
            var response = await client.PostAsync(GetTokenPath, authCredentials);
            var dto = await ReturnDto<AuthView>(response);

            return dto;
        }

        public async Task<HttpResponseDTO<AuthView>> RefreshToken(HttpClient client, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync(RefreshTokenPath);
            var dto = await ReturnDto<AuthView>(response);

            return dto;
        }

        private FormUrlEncodedContent AuthRequestConverter(AuthRequest authRequest)
        {
            Dictionary<string, string> authDictionary = new Dictionary<string, string>
            {
                {"EmpLogin", authRequest.EmpLogin},
                {"EmpPassword", authRequest.EmpPassword}
            };
            var authCredentials = new FormUrlEncodedContent(authDictionary);

            return authCredentials;
        }
    }
}
