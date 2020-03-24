using FinancialPortal.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FinancialPortal.Web.HttpHandlers
{
    public class OpenWrksIdentityTokenHandler:DelegatingHandler
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOpenWrksTokenProvider _openWorksTokenProvider;
        private readonly ILogger<OpenWrksIdentityTokenHandler> _logger;

        public OpenWrksIdentityTokenHandler(HttpClient client,
            IHttpContextAccessor httpContextAccessor,
            IOpenWrksTokenProvider openWorksTokenProvide,
            ILogger<OpenWrksIdentityTokenHandler> logger)
        {
            _client = client;
            _httpContextAccessor = httpContextAccessor;
            _openWorksTokenProvider = openWorksTokenProvide;
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
                string token = await _openWorksTokenProvider.GetAccessTokenAsync();

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
