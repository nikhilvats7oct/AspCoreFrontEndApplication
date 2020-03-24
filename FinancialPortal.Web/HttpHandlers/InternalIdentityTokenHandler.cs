using FinancialPortal.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FinancialPortal.Web.HttpHandlers
{
    public class InternalIdentityTokenHandler : DelegatingHandler
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAnonymousAccessTokenProvider _anonymousAccessTokenProvider;
        private readonly ILogger<InternalIdentityTokenHandler> _logger;

        public InternalIdentityTokenHandler(HttpClient client,
            IHttpContextAccessor httpContextAccessor,
            IAnonymousAccessTokenProvider anonymousAccessTokenProvider,
            ILogger<InternalIdentityTokenHandler> logger)
        {
            _client = client;
            _httpContextAccessor = httpContextAccessor;
            _anonymousAccessTokenProvider = anonymousAccessTokenProvider;
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
           HttpRequestMessage request,
           CancellationToken cancellationToken)
        {
            await SetupAuthHeader(request);

            return await base.SendAsync(request, cancellationToken);
        }

        private async Task SetupAuthHeader(HttpRequestMessage request)
        {
            try
            {
                string token = null;

                if (_httpContextAccessor?.HttpContext?.User?.Identity != null &&
                    _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
                }

                token = token ?? await _anonymousAccessTokenProvider.GetAccessTokenAsync();

                if (request.Headers.Contains("Authorization"))
                {
                    request.Headers.Remove("Authorization");
                }

                request.Headers.Add("Authorization", $"Bearer {token}");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An error has occurred while setting up auth header to the http client.");
                throw;
            }
        }
    }
}
