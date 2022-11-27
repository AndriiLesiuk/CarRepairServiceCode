using System.Net.Http;
using System.Threading.Tasks;
using CarRepairServiceCode.IntegrationTests.AuthorizationTests;
using CarRepairServiceCode.IntegrationTests.EmpPositionsTests;
using CarRepairServiceCode.IntegrationTests.TestConfig;
using CarRepairServiceCode.RequestModels.Authorization;

namespace CarRepairServiceCode.IntegrationTests.BaseEntities
{
    public class BaseFixture : WebApplicationFactoryWithInMemorySqlite
    {
        private const string ManagerMail = "johndevidson@gmail.com";
        private const string MachinistMail = "bobdownson@gmail.com";
        private const string ManagerPassword = "john12345";
        private const string MachinistPassword = "bob12345";

        public readonly string TokenWithPermissions;
        public readonly string TokenWithoutPermissions;

        public readonly AuthorizationClient AuthClient;

        public readonly AuthRequest AuthenticationDataWithPermissions = new AuthRequest
        {
            EmpLogin = ManagerMail,
            EmpPassword = ManagerPassword
        };
        public readonly AuthRequest AuthenticationDataWithoutPermissions = new AuthRequest
        {
            EmpLogin = MachinistMail,
            EmpPassword = MachinistPassword
        };

        public BaseFixture()
        {
            AuthClient = new AuthorizationClient();
            TokenWithPermissions = GenerateToken(AuthenticationDataWithPermissions).Result;
            TokenWithoutPermissions = GenerateToken(AuthenticationDataWithoutPermissions).Result;
        }

        protected async Task<string> GenerateToken(AuthRequest authenticationData)
        {
            var response = await AuthClient.GenerateTokenForEmployee(_client, authenticationData);
            var token = response.View.Token;

            return token;
        }
    }
}
