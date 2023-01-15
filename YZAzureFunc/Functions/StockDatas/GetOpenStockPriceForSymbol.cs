using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using YZAzureFunc.Function.Domain.Helpers.HttpRequestDataHelpers;
using YZAzureFunc.Function.Domain.Providers.StockDatas;

namespace YZAzureFunc.Functions.StockDatas
{
    // REF: https://medium.com/@manuelspinto/create-a-complete-azure-function-project-in-net-6-and-af-v4-bd1cc714452c
    // regarding the certificate https://stackoverflow.com/questions/62453695/how-to-enable-azure-function-https-easily-when-do-local-test,
    // https://www.koskila.net/azure-functions-host-quits-with-the-system-cannot-find-the-file-specified/
    // to debug locally refer tasks.json but can use func host start --port 7071 --useHttps --cors * --cert certificate.pfx --password 123
    // to allow invalid cert loaded from localhost https://stackoverflow.com/questions/47700269/google-chrome-localhost-neterr-cert-authority-invalid
    public class GetOpenStockPriceForSymbol
    {
        private readonly IStockDataProvider _stockDataProvider;
        private readonly IHttpRequestDataHelper _httpRequestDataHelper;
        private readonly ILogger<GetOpenStockPriceForSymbol> _logger;

        public GetOpenStockPriceForSymbol(IStockDataProvider stockDataProvider, IHttpRequestDataHelper httpRequestDataHelper, ILoggerFactory loggerFactory)
        {
            _stockDataProvider = stockDataProvider;
            _httpRequestDataHelper = httpRequestDataHelper;
            _logger = loggerFactory.CreateLogger<GetOpenStockPriceForSymbol>();
        }

        [Function("GetOpenStockPriceForSymbol")]
        [OpenApiOperation(operationId: "GetOpenStockPriceForSymbol", tags: new[] { "stock-price/symbol" })]
        [OpenApiParameter(name: "symbol", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "Symbol to get stock data from")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "OK response")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "stock-price/symbol/{symbol:alpha}/open")] HttpRequestData req, string symbol)
        {
            _logger.LogInformation($"Getting open stock price for symbol: {symbol}");

            var openPrice = await GetOpenStockPriceForSymbolAsync(symbol);

            var response = await _httpRequestDataHelper.CreateSuccessfulHttpResponse(req, openPrice);
            return response;
        }

        private async Task<decimal> GetOpenStockPriceForSymbolAsync(string symbol)
        {
            var stockData = await _stockDataProvider.GetStockDataForSymbolAsync(symbol);
            var openPrice = stockData.Open;

            return openPrice;
        }

        //private async Task<string> test(string symbol)
        //{
        //    var client = new StockPriceClient(new HttpClient());
        //    var result = client.GetOpenStockPriceForSymbolAsync("APPL").Result;

        //    return result;
        //}
    }
}
