namespace YZAzureFunc.Function.Domain.Helpers.EndPointHelpers
{
    public interface IEndPointHelper
    {
        public string GetBaseUrl();
        public string GetDirectory(string functionName);
        public string GetEndpoint(string functionName);
    }
}
