using System.Net;
using System.Net.Http;

namespace CarRepairServiceCode.IntegrationTests.BaseEntities
{
    public class HttpResponseDTO<T>
    {
        public T View;
        public HttpRequestMessage RequestMessage;
        public HttpStatusCode StatusCode;
    }
}
