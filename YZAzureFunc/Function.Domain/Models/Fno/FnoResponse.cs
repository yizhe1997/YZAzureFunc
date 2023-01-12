using Newtonsoft.Json.Linq;
using System.Net;

namespace YZAzureFunc.Function.Domain.Models.Fno
{
    public class FnoResponse
    {
        public dynamic? responseEntity { get; set; }
        public JArray? responseEntities { get; set; }
        public HttpStatusCode responseCode { get; set; }
        public string? errorMessage { get; set; }
        public string? warningMessage { get; set; }
        public string? infoMessage { get; set; }
    }
}
