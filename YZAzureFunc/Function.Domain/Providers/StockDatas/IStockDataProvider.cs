using YZAzureFunc.Function.Domain.Models.StockDatas;

namespace YZAzureFunc.Function.Domain.Providers.StockDatas
{
    public interface IStockDataProvider
    {
        public Task<StockData> GetStockDataForSymbolAsync(string symbol);
    }
}