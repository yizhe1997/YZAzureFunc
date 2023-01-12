namespace YZAzureFunc.Function.Domain.Authentication
{
    public interface IAzureAdAppProxy
    {
        public Task<string> GetAccessToken();
    }
}
