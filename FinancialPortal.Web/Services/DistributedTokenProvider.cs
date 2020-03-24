using FinancialPortal.Web.Services.Interfaces;
using System;
using System.Security;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;

namespace FinancialPortal.Web.Services
{
    public class DistributedTokenProvider : IDistributedTokenProvider
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DistributedTokenProvider(
            IDistributedCache distributedCache,
            IHttpContextAccessor httpContextAccessor)
        {
            _distributedCache = distributedCache;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> GetDistributedTokenId()
        {
            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");

            if (accessToken != null)
            {
                var id = Guid.NewGuid().ToString("N");

                await _distributedCache.SetStringAsync(id, accessToken, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2)
                });

                return id;
            }

            throw new SecurityException("Unable to find access token");
        }
    }
}
