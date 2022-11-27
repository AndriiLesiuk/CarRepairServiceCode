using CarRepairServiceCode.IntegrationTests.BaseEntities;
using CarRepairServiceCode.RequestModels.EmpPosition;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CarRepairServiceCode.IntegrationTests.EmpPositionsTests
{
    public class EmpPositionClient : BaseClient
    {
        protected const string AddPositionPath = "/EmpPosition/AddPosition/";
        protected const string GetPositionsPath = "/EmpPosition/GetPositions?PositionName=";
        protected const string GetPositionByIdPath = "/EmpPosition/GetPositionByid/";
        protected const string UpdatePositionPath = "/EmpPosition/UpdatePosition/";
        protected const string DeletePositionPath = "/EmpPosition/DeletePosition/";

        public async Task<HttpResponseDTO<EmpPositionView>> AddPosition(HttpClient client, string token, EmpPositionRequest empPositionRequest)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var posName = CreateHttpContentFromRequest(empPositionRequest);
            var response = await client.PostAsync(AddPositionPath, posName);
            var dto = await ReturnDto<EmpPositionView>(response);

            return dto;
        }

        public async Task<HttpResponseDTO<List<EmpPositionView>>> GetPositions(HttpClient client, string token, EmpPositionQuery empPositionQuery)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync(GetPositionsPath + empPositionQuery.PositionName);
            var dto = await ReturnDto<List<EmpPositionView>>(response);

            return dto;
        }

        public async Task<HttpResponseDTO<EmpPositionView>> GetPositionById(HttpClient client, string token, int positionId)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync(GetPositionByIdPath + positionId);
            var dto = await ReturnDto<EmpPositionView>(response);

            return dto;
        }

        public async Task<HttpResponseDTO<EmpPositionView>> UpdatePosition(HttpClient client, string token, int positionId, EmpPositionRequest empPositionRequest)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = CreateHttpContentFromRequest(empPositionRequest);
            var response = await client.PutAsync(UpdatePositionPath + positionId, request);
            var dto = await ReturnDto<EmpPositionView>(response);

            return dto;
        }

        public async Task<HttpResponseDTO<EmpPositionView>> DeletePosition(HttpClient client, string token, int positionId)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.DeleteAsync(DeletePositionPath + positionId);
            var dto = await ReturnDto<EmpPositionView>(response);

            return dto;
        }
    }
}
