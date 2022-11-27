using System.Net;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CarRepairServiceCode.IntegrationTests.BaseEntities
{
    public class BaseClient
    {
        protected HttpResponseDTO<T> DTOContentCreator<T>(HttpResponseMessage response, T contentData)
        {
            HttpResponseDTO<T> dto = new HttpResponseDTO<T>
            {
                View = contentData,
                RequestMessage = response.RequestMessage,
                StatusCode = response.StatusCode
            };

            return dto;
        }

        protected async Task<HttpResponseDTO<T>> ReturnDto<T>(HttpResponseMessage response) where T : class
        {
            if (response.IsSuccessStatusCode)
            {
                var contentData = await response.Content.ReadAsAsync<T>();
                var dto = DTOContentCreator(response, contentData);

                return dto;
            }
            else
            {
                var dto = DTOContentCreator<T>(response, null);
                return dto;
            }
        }

        protected HttpContent CreateHttpContentFromRequest<T>(T entity)
        {
            var json = JsonConvert.SerializeObject(entity);
            HttpContent payload = new StringContent(json, Encoding.UTF8, "application/json");

            return payload;
        }
    }
}
