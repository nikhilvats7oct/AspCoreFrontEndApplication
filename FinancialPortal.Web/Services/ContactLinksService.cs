using FinancialPortal.Web.Services.ApiModels;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Settings;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Services
{
    public class ContactLinksService : IContactLinksService
    {
        private readonly ILogger<ContactLinksService> _logger;
        private readonly IRestClient _restClient;
        private readonly PortalSetting _portalSettings;

        public ContactLinksService(ILogger<ContactLinksService> logger,
                                   IRestClient restClient,
                                   PortalSetting portalSettings)
        {
            _logger = logger;
            _restClient = restClient;
            _portalSettings = portalSettings;
        }

        public async Task Update(string id)
        {
            var url = $"{_portalSettings.GatewayEndpoint}api/ContactLinks";

            var date = DateTime.UtcNow.ToString("s") + "Z";
            var dto = new ContactlinksApiModel()
            {
                LinkId = id,
                LatestClick = date
            };

            await _restClient.PostNoResponseAsync(url, dto);
        }
    }
}
