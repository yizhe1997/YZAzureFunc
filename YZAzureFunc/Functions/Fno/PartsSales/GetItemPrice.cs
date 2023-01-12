using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using YZAzureFunc.Function.Domain.Helpers.HttpRequestDataHelpers;
using YZAzureFunc.Function.Domain.Models.Fno.PartsSales;
using YZAzureFunc.Function.Domain.Providers.Fno;

namespace YZAzureFunc.Functions.Fno.PartsSales
{
    public class GetItemPrice
    {
        private readonly IFnoProvider _fnoProvider;
        private readonly IHttpRequestDataHelper _httpRequestDataHelper;
        private readonly ILogger<GetItemPrice> _logger;

        public GetItemPrice(IFnoProvider fnoProvider, IHttpRequestDataHelper httpRequestDataHelper, ILoggerFactory loggerFactory)
        {
            _fnoProvider = fnoProvider;
            _httpRequestDataHelper = httpRequestDataHelper;
            _logger = loggerFactory.CreateLogger<GetItemPrice>();
        }

        [Function("GetItemPrice")]
        [OpenApiOperation(operationId: "GetItemPrice", tags: new[] { "stock-price/symbol" })]
        [OpenApiParameter(name: "Context-Token", In = ParameterLocation.Header, Required = true, Type = typeof(string), Description = "Context Token")]
        [OpenApiRequestBody("application/json", typeof(ProductIdentifier), Description = "JSON request body ProductIdentifier"), ]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response message containing a JSON result.")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData req, FunctionContext functionContext)
        {
            // Get request body data.
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var contextToken = _httpRequestDataHelper.GetContextToken(req);
            string functionName = functionContext.FunctionDefinition.Name;
            var data = JsonConvert.DeserializeObject<ProductIdentifier>(requestBody);

            // Return bad request if capacity or hours are not passed in
            if (data == null)
            {
                return await _httpRequestDataHelper.CreateBadRequestHttpResponse(req, "Failed to deserialize json as product identifier");
            }

            // Using fno provider get the item price respone
            _logger.LogInformation($"Getting item price price.");

            var fnoResponse = await _fnoProvider.GetItemPriceAsync(contextToken, data, functionName);

            // Handle function response based on fno response status code
            if (fnoResponse.responseCode == HttpStatusCode.OK)
            {
                return await _httpRequestDataHelper.CreateSuccessfulHttpResponse(req, fnoResponse);
            }
            else
            {
                return await _httpRequestDataHelper.CreateCustomHttpResponse(req, fnoResponse, fnoResponse.responseCode);
            }
        }
    }
}
