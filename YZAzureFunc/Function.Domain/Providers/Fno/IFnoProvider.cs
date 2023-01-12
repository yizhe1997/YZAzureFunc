using YZAzureFunc.Function.Domain.Models.Fno;
using YZAzureFunc.Function.Domain.Models.Fno.PartsSales;

namespace YZAzureFunc.Function.Domain.Providers.Fno
{
    public interface IFnoProvider
    {
        public Task<Response> GetItemPriceAsync(string contextToken, ProductIdentifier data, string functionName);
    }
}
