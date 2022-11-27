using CarRepairServiceCode.IntegrationTests.BaseEntities;
using CarRepairServiceCode.RequestModels.Authorization;
using System.Threading.Tasks;

namespace CarRepairServiceCode.IntegrationTests.AuthorizationTests
{
    public class AuthorizationFixture : BaseFixture
    {
        private readonly AuthorizationClient client;

        private static readonly string MechanicLogin = "bobdownson@gmail.com";
        private static readonly string MechanicPassword = "bob12345";

        public readonly string InvalidToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9";
        public readonly string ExpiredToken =
            "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJib2Jkb3duc29uQGdtYWlsLmNvbSIsImp0aSI6IjUxYjNlNjYyLTVjNGYtNGE2ZC05N2EyLTQxMGJlYTIzMjVhMSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IlVzZXIiLCJlbXBsb3llZV9pZCI6IjIiLCJmaXJzdF9uYW1lIjoiQm9iIiwibGFzdF9uYW1lIjoiRG93bnNvbiIsInBvc2l0aW9uX2lkIjoiMiIsImlzX2FjdGl2ZSI6IlRydWUiLCJleHAiOjE2MzcxNDY2OTMsImlzcyI6IkJhY2tFbmQgUGFydCBPZiBBcHBsaWNhdGlvbiIsImF1ZCI6IkZyb250RW5kIFBhcnQgT2YgQXBwbGljYXRpb24ifQ.xhB5MhPIaeISNXyHHLJbpJraPjDQuYgSivdPW1zj78I";

        public readonly AuthRequest AuthRequestWithExistCredentials = new AuthRequest
        {
            EmpLogin = MechanicLogin,
            EmpPassword = MechanicPassword
        };
        public readonly AuthRequest AuthRequestWithNotExistCredentials = new AuthRequest
        {
            EmpLogin = "testtext@mail.ru",
            EmpPassword = "test12345"
        };

        public AuthorizationFixture()
        {
            client = new AuthorizationClient();
        }

        public async Task<HttpResponseDTO<AuthView>> GenerateTokenForEmployee(AuthRequest authRequest)
        {
            var response = await client.GenerateTokenForEmployee(_client, authRequest);

            return response;
        }

        public async Task<HttpResponseDTO<AuthView>> RefreshTokenForEmployee(AuthRequest authRequest, string token)
        {
            var response = await client.RefreshToken(_client, token);

            return response;
        }
    }
}
