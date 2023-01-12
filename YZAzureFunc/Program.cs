using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using YZAzureFunc.Function.Domain.Helpers.HttpRequestDataHelpers;
using YZAzureFunc.Function.Domain.Helpers.HttpClientHelpers;
using YZAzureFunc.Function.Domain.Authentication;
using YZAzureFunc.Function.Domain.Providers.StockDatas;
using YZAzureFunc.Function.Domain.Helpers.EndPointHelpers;
using YZAzureFunc.Function.Domain.Services.HttpClients.Fno;
using YZAzureFunc.Function.Domain.Services.HttpClients.StockDatas;
using YZAzureFunc.Function.Domain.Providers.Fno;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(worker => worker.UseNewtonsoftJson())
    .ConfigureOpenApi()
    // Ref: https://stackoverflow.com/questions/38138100/addtransient-addscoped-and-addsingleton-services-differences#:~:text=Singleton%20is%20a%20single%20instance,HTTP%20request%20in%20ASP.NET.
    .ConfigureServices(s =>
    {
        #region Helpers

        s.AddScoped<IHttpRequestDataHelper, HttpRequestDataHelper>();
        s.AddScoped<IHttpClientHelper, HttpClientHelper>();

        #endregion

        #region Authentication

        s.AddScoped<IAzureAdAppProxy, AzureAdAppProxy>();

        #endregion

        #region HttpClients

        s.AddHttpClient<FinhubHttpClient>();
        s.AddHttpClient<FnoHttpClient>();

        #endregion

        #region Misc.

        s.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        #endregion

        #region Provider Endpoints

        s.AddScoped<IFnoEndPointHelper, FnoEndPointHelper>();

        #endregion

        #region Providers

        s.AddScoped<IStockDataProvider, FinhubProvider>();
        s.AddScoped<IFnoProvider, FnoProvider>();

        #endregion
    })
    .Build();

host.Run();