using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Settings;
using IdentityModel.Client;
using Microsoft.Extensions.Caching.Distributed;

namespace FinancialPortal.Web.Services
{
    public class AnonymousAccessTokenProvider : IAnonymousAccessTokenProvider
    {
        public string Key = "Lfl.Anonymous.AccessToken";
        private readonly IDiscoveryCache _discoveryCache;
        private readonly HttpClient _httpClient;
        private readonly IdentitySetting _identitySetting;
        private readonly IDistributedCache _distributedCache;

        public AnonymousAccessTokenProvider(
            IDistributedCache distributedCache,
            IDiscoveryCache discoveryCache,
            HttpClient httpClient,
            IdentitySetting identitySetting)
        {
            _distributedCache = distributedCache;
            _discoveryCache = discoveryCache;
            _httpClient = httpClient;
            _identitySetting = identitySetting;

            Key += Environment.GetEnvironmentVariable("COMPUTERNAME");
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var cachedTokenResponseBytes = await _distributedCache.GetAsync(Key);

            if (cachedTokenResponseBytes != null)
            {
                var accessToken = Encoding.UTF8.GetString(cachedTokenResponseBytes);

                if (!string.IsNullOrWhiteSpace(accessToken))
                {
                    return accessToken;
                }
            }

            var tokenResponse = await GetAccessTokenFromIdentityAsync();

            await _distributedCache.SetAsync(
                Key,
                Encoding.UTF8.GetBytes(tokenResponse.AccessToken),
                new DistributedCacheEntryOptions
                {
                    // Expire a minute before the token expires.
                    AbsoluteExpiration = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn - 60)
                });
            return tokenResponse.AccessToken;
        }

        private async Task<TokenResponse> GetAccessTokenFromIdentityAsync()
        {
            var discovery = await _discoveryCache.GetAsync();

            if (discovery.IsError)
            {
                throw new Exception(discovery.Error);
            }

            var request = new PasswordTokenRequest
            {
                ClientId = _identitySetting.ClientId,
                ClientSecret = _identitySetting.ClientSecret,
                Address = discovery.TokenEndpoint,
                GrantType = "password",
                UserName = _identitySetting.AnonymousCredentialUsername,
                Password = _identitySetting.AnonymousCredentialPassword,
                Scope = string.Join(" ", _identitySetting.Scopes)
            };

            var tokenResponse = await _httpClient.RequestPasswordTokenAsync(request);

            if (tokenResponse.IsError)
            {
                throw new Exception(tokenResponse.Error + tokenResponse.ErrorDescription);
            }

            return tokenResponse;
        }
    }
}
