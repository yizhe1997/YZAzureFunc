using System.Net.Http.Json;
using YZAzureFunc.Function.Domain.Helpers.HttpClientHelpers;
using YZAzureFunc.Function.Domain.Models.StockDatas;

namespace YZAzureFunc.Function.Domain.Services.HttpClients.StockDatas
{
    public class FinhubHttpClient
    {
        public HttpClient _client { get; }
        private readonly IHttpClientHelper _httpClientHelper;

        public FinhubHttpClient(HttpClient client, IHttpClientHelper clientHelper)
        {
            _httpClientHelper = clientHelper;

            client = _httpClientHelper.ClientSetBaseAddress(client, Environment.GetEnvironmentVariable("FinhubApiBaseUrl") ?? "");
            client = _httpClientHelper.ClientAddCustomeHeader(client, "X-Finnhub-Token", Environment.GetEnvironmentVariable("FinhubApiToken") ?? "");

            _client = client;
        }

        public async Task<FinhubStockData> GetStockDataForSymbolAsync(string symbol)
        {
            var quotePath = $"quote?symbol={symbol}";

            var response = await _client.GetAsync(quotePath);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<FinhubStockData>() ?? new();
        }
    }
}