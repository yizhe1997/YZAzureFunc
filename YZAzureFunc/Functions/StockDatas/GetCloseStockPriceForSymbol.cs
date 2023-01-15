using System.Net;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;
using Microsoft.Azure.Functions.Worker;
using YZAzureFunc.Function.Domain.Helpers.HttpRequestDataHelpers;
using YZAzureFunc.Function.Domain.Providers.StockDatas;

namespace YZAzureFunc.Functions.StockDatas
{
    public class GetCloseStockPriceForSymbol
    {
        private readonly IStockDataProvider _stockDataProvider;
        private readonly IHttpRequestDataHelper _httpRequestDataHelper;
        private readonly ILogger<GetOpenStockPriceForSymbol> _logger;

        public GetCloseStockPriceForSymbol(
                    IStockDataProvider stockDataProvider,
                    IHttpRequestDataHelper httpRequestDataHelper,
                    ILogger<GetOpenStockPriceForSymbol> logger)
        {
            _stockDataProvider = stockDataProvider;
            _httpRequestDataHelper = httpRequestDataHelper;
            _logger = logger;
        }

        [Function("GetCloseStockPriceForSymbol")]
        [OpenApiOperation(operationId: "GetCloseStockPriceForSymbol", tags: new[] { "stock-price/symbol" })]
        [OpenApiParameter(name: "symbol", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "Symbol to get stock data from")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "OK response")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "stock-price/symbol/{symbol:alpha}/close")] HttpRequestData req, string symbol)
        {
            _logger.LogInformation($"Getting previous close stock price for symbol: {symbol}");

            var closePrice = await GetCloseStockPriceForSymbolAsync(symbol);

            var response = await _httpRequestDataHelper.CreateSuccessfulHttpResponse(req, closePrice);
            return response;
        }

        private async Task<decimal> GetCloseStockPriceForSymbolAsync(string symbol)
        {
            var stockData = await _stockDataProvider.GetStockDataForSymbolAsync(symbol);
            var closePrice = stockData.PreviousClose;

            return closePrice;
        }
    }
}
