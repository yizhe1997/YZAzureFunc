using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace YZAzureFunc.Function.Domain.Helpers.HttpRequestDataHelpers
{
    public interface IHttpRequestDataHelper
    {
        public string GetContextToken(HttpRequestData req);
        public string GetRoute(HttpRequestData req);
        public Task<HttpResponseData> CreateSuccessfulHttpResponse(HttpRequestData req, object data);
        public Task<HttpResponseData> CreateBadRequestHttpResponse(HttpRequestData req, object data);
        public Task<HttpResponseData> CreateCustomHttpResponse(HttpRequestData req, object data, HttpStatusCode responseCode);
    }
}
