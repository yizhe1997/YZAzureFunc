using Microsoft.Identity.Client;

namespace YZAzureFunc.Function.Domain.Authentication
{
    public class AzureAdAppProxy : IAzureAdAppProxy
    {
        // Ref: https://learn.microsoft.com/en-us/azure/active-directory/develop/scenario-daemon-acquire-token?tabs=dotnet,
        // https://learn.microsoft.com/en-us/azure/active-directory/app-proxy/application-proxy
        public async Task<string> GetAccessToken()
        {
            string authority = Environment.GetEnvironmentVariable("AzureAdAuthority") ?? "";
            string fnoApiBaseUrl = Environment.GetEnvironmentVariable("FnoApiBaseUrl") ?? "";
            string clientId = Environment.GetEnvironmentVariable("AzureAdClientId") ?? "";
            string clientSecret = Environment.GetEnvironmentVariable("AzureAdClientSecret") ?? "";

            var clientCredential = ConfidentialClientApplicationBuilder.Create(clientId)
                .WithAuthority(authority, false)
                .WithClientSecret(clientSecret)
                .Build();

            // Ref: https://learn.microsoft.com/en-us/azure/active-directory/develop/v2-oauth2-client-creds-grant-flow
            string[] scopes = new string[] { $"{fnoApiBaseUrl}/.default" };
            AuthenticationResult? authenticationResult = null;

            try
            {
                authenticationResult = await clientCredential.AcquireTokenForClient(scopes).ExecuteAsync();
            }
            catch (MsalUiRequiredException)
            {
                // The application doesn't have sufficient permissions.
                // - Did you declare enough app permissions during app creation?
                // - Did the tenant admin grant permissions to the application?
            }
            catch (MsalServiceException ex) when (ex.Message.Contains("AADSTS70011"))
            {
                // Invalid scope. The scope has to be in the form "https://resourceurl/.default"
                // Mitigation: Change the scope to be as expected.
            }

            return authenticationResult?.AccessToken ?? "";
        }
    }
}
