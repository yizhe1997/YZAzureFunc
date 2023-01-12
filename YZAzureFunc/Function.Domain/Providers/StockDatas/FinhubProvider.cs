using AutoMapper;
using YZAzureFunc.Function.Domain.Models.StockDatas;
using YZAzureFunc.Function.Domain.Services.HttpClients.StockDatas;

namespace YZAzureFunc.Function.Domain.Providers.StockDatas
{
    public class FinhubProvider : IStockDataProvider
    {
        private readonly FinhubHttpClient _client;
        private readonly IMapper _mapper;

        public FinhubProvider(FinhubHttpClient client, IMapper mapper)
        {
            _client = client;
            _mapper = mapper;
        }

        public async Task<StockData> GetStockDataForSymbolAsync(string symbol)
        {
            var stockDataRaw = await _client.GetStockDataForSymbolAsync(symbol);

            return _mapper.Map<StockData>(stockDataRaw);
        }
    }
}