using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Clients.ActiveDirectory;


namespace AzImmersiveReader.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImmersiveReaderController : ControllerBase
    {
        private readonly string TenantId;
        private readonly string ClientId;
        private readonly string ClientSecret;
        private readonly string Subdomain;

        public ImmersiveReaderController(Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            TenantId = configuration["TenantId"];
            ClientId = configuration["ClientId"];
            ClientSecret = configuration["ClientSecret"];
            Subdomain = configuration["Subdomain"];

            if (string.IsNullOrWhiteSpace(TenantId))
            {
                throw new ArgumentNullException("TenantId is null! Did you add that info to secrets.json?");
            }

            if (string.IsNullOrWhiteSpace(ClientId))
            {
                throw new ArgumentNullException("ClientId is null! Did you add that info to secrets.json?");
            }

            if (string.IsNullOrWhiteSpace(ClientSecret))
            {
                throw new ArgumentNullException("ClientSecret is null! Did you add that info to secrets.json?");
            }

            if (string.IsNullOrWhiteSpace(Subdomain))
            {
                throw new ArgumentNullException("Subdomain is null! Did you add that info to secrets.json?");
            }
        }

        /// <summary>
        /// Get an Azure AD authentication token.
        /// </summary>
        private async Task<string> GetTokenAsync()
        {
            string authority = $"https://login.windows.net/{TenantId}";
            const string resource = "https://cognitiveservices.azure.com/";

            AuthenticationContext authContext = new AuthenticationContext(authority);
            ClientCredential clientCredential = new ClientCredential(ClientId, ClientSecret);

            AuthenticationResult authResult = await authContext.AcquireTokenAsync(resource, clientCredential);

            return authResult.AccessToken;
        }


        // GET: api/ImmersiveReader
        [HttpGet]
        public async Task<JsonResult> GetTokenAndSubdomain()
        {
            try
            {
                string tokenResult = await GetTokenAsync();
                return new JsonResult(new { token = tokenResult, subdomain = Subdomain });
            }
            catch (Exception e)
            {
                string message = "Unable to acquire Azure AD token. Check the debugger for more information.";
                Debug.WriteLine(message, e);
                return new JsonResult(new { error = message });
            }
        }
    }
}
