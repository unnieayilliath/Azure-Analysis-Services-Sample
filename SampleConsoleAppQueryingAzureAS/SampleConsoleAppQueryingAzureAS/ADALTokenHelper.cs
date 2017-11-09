using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace SampleConsoleAppQueryingAzureAS
{
    class ADALTokenHelper
    {
        private static string _authUrl = "https://login.windows.net";
        /// <summary>
        /// App only access token is used to get tokens for daemon clients.
        /// </summary>
        /// <param name="resourceUrl"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <returns></returns>
        public async static Task<string> GetAppOnlyAccessToken(string domain, string resourceUrl,
            string clientId,
            string clientSecret)
        {
            var authority = $"{_authUrl}/{domain}/oauth2/token";
            var authContext = new AuthenticationContext(authority);
            // Config for OAuth client credentials 
            var clientCred = new ClientCredential(clientId, clientSecret);
            AuthenticationResult authenticationResult = await authContext.AcquireTokenAsync(resourceUrl, clientCred);
            //get access token
            return authenticationResult.AccessToken;
        }
    }
}
