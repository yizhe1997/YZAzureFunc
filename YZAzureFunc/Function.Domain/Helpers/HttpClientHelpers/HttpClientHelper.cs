using System.Net.Http.Headers;

namespace YZAzureFunc.Function.Domain.Helpers.HttpClientHelpers
{
    public class HttpClientHelper : IHttpClientHelper
    {
        public HttpClient ClientAddAuthorizationBearer(HttpClient httpClient, string token)
        {

            var authValue = new AuthenticationHeaderValue("Bearer", token);

            httpClient.DefaultRequestHeaders.Authorization = authValue;

            return httpClient;
        }

        //public async Task<HttpClient> GetClient()
        //{

        //    var accessToken = await Authentication.GetS2SAccessTokenForProdMSAAsync();

        //    return HttpClientHelper.ClientAddAuthorizationBearer(accessToken.AccessToken);
        //}

        public HttpClient ClientAddCustomeHeader(HttpClient httpClient, string headerKey, string headerValue)
        {

            httpClient.DefaultRequestHeaders.Add(headerKey, headerValue);

            return httpClient;
        }

        public HttpClient ClientSetBaseAddress(HttpClient httpClient, string baseAddress)
        {

            httpClient.BaseAddress = new Uri(baseAddress);

            return httpClient;
        }
    }
}
