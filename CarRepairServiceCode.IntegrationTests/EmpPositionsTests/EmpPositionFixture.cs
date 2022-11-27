using CarRepairServiceCode.IntegrationTests.BaseEntities;
using CarRepairServiceCode.RequestModels.EmpPosition;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRepairServiceCode.IntegrationTests.EmpPositionsTests
{
    public class EmpPositionFixture : BaseFixture
    {
        private readonly EmpPositionClient client;

        public readonly int EmpPositionEmptyListCount = 0;
        public readonly int EmpPositionListForExistQuery = 2;

        public readonly int EmpPositionManagerId = 1;
        public readonly int EmpPositionMechanicId = 2;
        public readonly int EmpPositionAccountantId = 4;
        public readonly int NotExistEmpPositionId = 99;

        public static readonly string Manager = "Manager";
        public const string Mechanic = "Mechanic";
        public static readonly string Maid = "Maid";
        public static readonly string Painter = "Painter";

        public readonly EmpPositionRequest EmpPositionRequestPainter = new EmpPositionRequest
        {
            PositionName = Painter
        };

        public readonly EmpPositionQuery EmptyQuery = new EmpPositionQuery
        {
            PositionName = ""
        };
        public readonly EmpPositionQuery ExistQuery = new EmpPositionQuery
        {
            PositionName = "Ma"
        };
        public readonly EmpPositionQuery NotExistQuery = new EmpPositionQuery
        {
            PositionName = "WWWWW"
        };

        public readonly EmpPositionView EmpPositionViewManager = new EmpPositionView
        {
            PositionName = Manager
        };
        public readonly EmpPositionView EmpPositionViewMechanic = new EmpPositionView
        {
            PositionName = Mechanic
        };
        public readonly EmpPositionView EmpPositionViewMaid = new EmpPositionView
        {
            PositionName = Maid
        };
        public readonly EmpPositionView EmpPositionViewPainter = new EmpPositionView
        {
            PositionName = Painter
        };

        public EmpPositionFixture()
        {
            client = new EmpPositionClient();
        }

        public async Task<HttpResponseDTO<EmpPositionView>> AddPosition(EmpPositionRequest empPositionRequest, string token)
        {
            var response = await client.AddPosition(_client, token, empPositionRequest);

            return response;
        }

        public async Task<HttpResponseDTO<List<EmpPositionView>>> GetPositions(EmpPositionQuery empPositionQuery, string token)
        {
            var response = await client.GetPositions(_client, token, empPositionQuery);

            return response;
        }

        public async Task<HttpResponseDTO<EmpPositionView>> GetPositionById(int positionId, string token)
        {
            var response = await client.GetPositionById(_client, token, positionId);

            return response;
        }

        public async Task<HttpResponseDTO<EmpPositionView>> UpdatePosition(int positionId, EmpPositionRequest empPositionRequest, string token)
        {
            var response = await client.UpdatePosition(_client, token, positionId, empPositionRequest);

            return response;
        }

        public async Task<HttpResponseDTO<EmpPositionView>> DeletePosition(int positionId, string token)
        {
            var response = await client.DeletePosition(_client, token, positionId);

            return response;
        }
    }
}
