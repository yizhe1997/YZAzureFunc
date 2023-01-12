namespace YZAzureFunc.Function.Domain.Helpers.HttpClientHelpers
{
    public interface IHttpClientHelper
    {
        public HttpClient ClientAddAuthorizationBearer(HttpClient httpClient, string token);
        public HttpClient ClientAddCustomeHeader(HttpClient httpClient, string headerKey, string headerValue);
        public HttpClient ClientSetBaseAddress(HttpClient httpClient, string baseAddress);
    }
}
