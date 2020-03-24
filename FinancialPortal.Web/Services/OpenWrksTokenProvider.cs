using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Settings;
using IdentityModel.Client;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Services
{
    public class OpenWrksTokenProvider : IOpenWrksTokenProvider
    {
        public string Key = "Lfl.OpenWrks.AccessToken";
        private readonly HttpClient _httpClient;
        private readonly OpenWrksSetting _openWrksSetting;
        private readonly IDistributedCache _distributedCache;

        public OpenWrksTokenProvider(
            IDistributedCache distributedCache,
            HttpClient httpClient,
            OpenWrksSetting openWrksSetting)
        {
            _distributedCache = distributedCache;
            _httpClient = httpClient;
            _openWrksSetting = openWrksSetting;

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
            var request = new TokenRequest
            {
                ClientId = _openWrksSetting.ClientId,
                ClientSecret = _openWrksSetting.ClientSecret,
                Address = $"{_openWrksSetting.Authority}/connect/token",
                GrantType = "client_credentials"            
            };

            var tokenResponse = await _httpClient.RequestTokenAsync(request);

            if (tokenResponse.IsError)
            {
                throw new Exception(tokenResponse.Error + tokenResponse.ErrorDescription);
            }

            return tokenResponse;
        }
    }
}
