using System.Net;

namespace YZAzureFunc.Function.Domain.Models.Fno
{
    public class Response
    {
        public dynamic? data { get; set; }
        public HttpStatusCode responseCode { get; set; }
        public string? errorMessage { get; set; }
        public string? warningMessage { get; set; }
        public string? infoMessage { get; set; }
    }
}
