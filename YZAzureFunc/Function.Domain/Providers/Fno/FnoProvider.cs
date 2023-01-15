using AutoMapper;
using Newtonsoft.Json.Linq;
using YZAzureFunc.Function.Domain.Helpers.EndPointHelpers;
using YZAzureFunc.Function.Domain.Models.Fno;
using YZAzureFunc.Function.Domain.Models.Fno.PartsSales;
using YZAzureFunc.Function.Domain.Services.HttpClients.Fno;

namespace YZAzureFunc.Function.Domain.Providers.Fno
{
    public class FnoProvider : IFnoProvider
    {
        private readonly FnoHttpClient _client;
        private readonly IMapper _mapper;
        private readonly IFnoEndPointHelper _fnoEndPointHelper;

        public FnoProvider(FnoHttpClient client, IMapper mapper, IFnoEndPointHelper fnoEndPointHelper)
        {
            _client = client;
            _mapper = mapper;
            _fnoEndPointHelper = fnoEndPointHelper;
        }

        public async Task<Response> GetItemPriceAsync(string contextToken, ProductIdentifier productIdentifier, string functionName)
        {
            // Get endpoint via function name
            var endPoint = _fnoEndPointHelper.GetEndpoint(functionName);

            // Map input for fno client
            dynamic parameter = new JObject();

            dynamic prodIdentifier = new JObject();
            prodIdentifier.itemId = productIdentifier.itemId;
            prodIdentifier.colorId = productIdentifier.colorId;
            prodIdentifier.sizeId = productIdentifier.sizeId;
            prodIdentifier.configId = productIdentifier.configId;
            prodIdentifier.name = productIdentifier.name;
            prodIdentifier.brandId = productIdentifier.brandId;
            prodIdentifier.classId = productIdentifier.classId;
            prodIdentifier.modelId = productIdentifier.modelId;
            prodIdentifier.styleId = productIdentifier.styleId;
            prodIdentifier.orderType = productIdentifier.orderType;
            prodIdentifier.qty = productIdentifier.qty;
            prodIdentifier.siteId = productIdentifier.siteId;
            prodIdentifier.warehouseId = productIdentifier.warehouseId;
            prodIdentifier.supplierItemId = productIdentifier.supplierItemId;
            prodIdentifier.supplementaryItemId = productIdentifier.supplementaryItemId;
            prodIdentifier.deviceOrderType = productIdentifier.deviceOrderType;

            parameter._contextToken = contextToken;
            parameter._productIdentifier = prodIdentifier;

            // Return response received from fno client 
            var fnoResponse = await _client.GetFnoResponseAsync(endPoint, parameter);
            var result = _mapper.Map<Response>(fnoResponse);
            return result;
        }
    }
}
