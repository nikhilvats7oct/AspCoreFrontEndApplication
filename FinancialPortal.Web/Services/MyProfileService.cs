using System;
using System.Threading.Tasks;
using AutoMapper;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Settings;
using Microsoft.Extensions.Logging;

namespace FinancialPortal.Web.Services
{
    public class MyProfileService : IMyProfileService
    {
        private readonly ILogger<MyProfileService> _logger;
        private readonly PortalSetting _portalSettings;
        private readonly IRestClient _restClient;
        private readonly IMapper _mapper;
        private readonly IAccountsService _accountsService;

        public MyProfileService(ILogger<MyProfileService> logger,
                                PortalSetting portalSettings,
                                IRestClient restClient,
                                IMapper mapper,
                                IAccountsService accountsService)
        {
            _logger = logger;
            _portalSettings = portalSettings;
            _restClient = restClient;
            _mapper = mapper;
            _accountsService = accountsService;
        }

        public async Task<ContactPreferencesRetrievedDto> GetContactPreferences(string lowellReference)
        {
            try
            {
                var url = $"{_portalSettings.GatewayEndpoint}api/MyProfile/GetContactPreferences";
                var result = await _restClient.PostAsync<RetrieveContactPreferencesDto, ContactPreferencesRetrievedDto>(url, new RetrieveContactPreferencesDto() { LowellReference = lowellReference });

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in Getting Customer Preferences.");
                throw;
            }
        }

        public async Task SaveContactPreferences(SaveContactPreferencesDto model)
        {
            try
            {

                var url = $"{_portalSettings.GatewayEndpoint}api/MyProfile/SaveContactPreferences";
                await _restClient.PostNoResponseAsync(url, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in Saving Customer Preferences.");
                throw;
            }
        }

    }
}
