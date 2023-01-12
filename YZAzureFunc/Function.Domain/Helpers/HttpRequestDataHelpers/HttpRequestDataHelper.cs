using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace YZAzureFunc.Function.Domain.Helpers.HttpRequestDataHelpers
{
    public class HttpRequestDataHelper : IHttpRequestDataHelper
    {
        public string GetContextToken(HttpRequestData req)
        {
            string resp = "";

            if (req.Headers.Contains("Context-Token"))
            {
                return req.Headers.GetValues("Context-Token")?.FirstOrDefault() ?? resp;
            }
            else
            {
                return resp;
            }
        }

        public string GetRoute(HttpRequestData req)
        {
            return req.Url.ToString();
        }

        public async Task<HttpResponseData> CreateSuccessfulHttpResponse(HttpRequestData req, object data)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(data);

            return response;
        }

        public async Task<HttpResponseData> CreateBadRequestHttpResponse(HttpRequestData req, object data)
        {
            var response = req.CreateResponse(HttpStatusCode.BadRequest);
            await response.WriteAsJsonAsync(data);

            return response;
        }

        public async Task<HttpResponseData> CreateCustomHttpResponse(HttpRequestData req, object data, HttpStatusCode responseCode)
        {
            var response = req.CreateResponse(responseCode);
            await response.WriteAsJsonAsync(data);

            return response;
        }
    }
}
