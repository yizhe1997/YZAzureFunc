using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Json;
using YZAzureFunc.Function.Domain.Authentication;
using YZAzureFunc.Function.Domain.Helpers.HttpClientHelpers;
using YZAzureFunc.Function.Domain.Models.Fno;

namespace YZAzureFunc.Function.Domain.Services.HttpClients.Fno
{
    public class FnoHttpClient
    {
        public HttpClient _client { get; }
        private readonly IHttpClientHelper _httpClientHelper;
        private readonly IAzureAdAppProxy _azureAdAppProxy;

        public FnoHttpClient(HttpClient client, IHttpClientHelper clientHelper, IAzureAdAppProxy azureAdAppProxy)
        {
            _azureAdAppProxy = azureAdAppProxy;
            _httpClientHelper = clientHelper;

            client = _httpClientHelper.ClientSetBaseAddress(client, Environment.GetEnvironmentVariable("FnoApiBaseUrl") ?? "");
            _client = client;
        }

        public async Task<HttpClient> GetAzureAdAppProxyAccessTokenForClientAsync(HttpClient client)
        {
            var azureAdAppProxyAccessToken = await _azureAdAppProxy.GetAccessToken();

            client = _httpClientHelper.ClientAddAuthorizationBearer(client, azureAdAppProxyAccessToken);

            return client;
        }

        public async Task<FnoResponse> GetFnoResponseAsync(string url, JObject parameter)
        {
            // Get azure ad app proxy access token for fno client
            await GetAzureAdAppProxyAccessTokenForClientAsync(_client); // dk if it will be assigned

            // Get response from fno
            var response = await _client.PostAsJsonAsync(url, parameter);

            // Handle response based on status code received from client
            FnoResponse data = new FnoResponse();

            if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
            {
                data.responseCode = response.StatusCode;
                string error = await response.Content.ReadAsStringAsync();
                data.errorMessage = error;
                return data;
            }
            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                data.responseCode = response.StatusCode;
                string message = await response.Content.ReadAsStringAsync();
                string error = $"There is error processing the request - {message}";
                data.errorMessage = error;
                return data;
            }
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                data.responseCode = response.StatusCode;
                string error = "FO service not found.";
                data.errorMessage = error;
                return data;
            }
            if (response.StatusCode == (HttpStatusCode)429)
            {
                data.responseCode = response.StatusCode;
                string error = await response.Content.ReadAsStringAsync();
                data.errorMessage = error;
                return data;
            }

            // ref: https://code-maze.com/csharp-deserialize-json-into-dynamic-object/
            // https://stackoverflow.com/questions/63121877/dynamic-c-sharp-valuekind-object
            string jsonString = await response.Content.ReadAsStringAsync();
            data = JsonConvert.DeserializeObject<FnoResponse>(jsonString) ?? new();

            return data;
        }
    }
}
