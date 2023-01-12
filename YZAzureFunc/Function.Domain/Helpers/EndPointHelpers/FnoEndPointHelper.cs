namespace YZAzureFunc.Function.Domain.Helpers.EndPointHelpers
{
    public class FnoEndPointHelper : IFnoEndPointHelper
    {
        public string GetBaseUrl()
        {
            return Environment.GetEnvironmentVariable("resource") ?? "";
        }

        public string GetDirectory(string functionName)
        {
            return Environment.GetEnvironmentVariable(functionName) ?? "";
        }

        public string GetEndpoint(string functionName)
        {
            // use try get?
            return $"{GetBaseUrl()}/{GetDirectory(functionName)}";
        }
    }
}
