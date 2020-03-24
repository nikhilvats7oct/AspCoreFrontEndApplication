using AutoMapper;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Services.Models;
using FinancialPortal.Web.Settings;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Threading.Tasks;
using FinancialPortal.Web.Services.Models.OpenWrks;
using System.Net;

namespace FinancialPortal.Web.Services
{
    public class OpenWrksService : IOpenWrksService
    {
        private readonly ILogger<OpenWrksService> _logger;
        private readonly PortalSetting _portalSetting;
        private readonly IRestClient _restClient;
        private readonly OpenWrksSetting _openWrksSetting;
        private readonly IMapper _mapper;

        public OpenWrksService(
            PortalSetting portalSetting,
            IRestClient restClient,
            OpenWrksSetting openWrksSetting,
            ILogger<OpenWrksService> logger, IMapper mapper)
        {
            _logger = logger;
            _portalSetting = portalSetting;
            _restClient = restClient;
            _openWrksSetting = openWrksSetting;
            _mapper = mapper;
        }

        public async Task<OpenWrksInvitationResponse> SendInvitationRequest(OpenWrksInvitationRequest request)
        {
            _logger.LogInformation("Requesting Invitation Link to OpenWrks.");

            var uri = $"{_openWrksSetting.ApiEndpoint}/v1/invite";

            _logger.LogDebug("Retrieving invitation link from OpenWrks");

            var response = await _restClient.PostAsync<OpenWrksInvitationRequest, OpenWrksInvitationResponse>(uri, request);

            _logger.LogInformation("Invitation link was returned successfully.");

            return response;
        }

        public async Task<IncomeAndExpenditure> GetOpenWorksBudgetTranslatedToCaseflowBudgetModel(string customerReference)
        {
            _logger.LogInformation("Requesting OpenWorks Budget.");

            OpenWrksBudget result;

            if (_openWrksSetting.EnableMockDataForTesting)
            {
                result = await GetBudget(customerReference);
            }
            else
            {
                var uri = $"{_openWrksSetting.ApiEndpoint}/v1/customer/{customerReference}/budget";

                _logger.LogDebug("Retrieving budget from OpenWrks");

                result = await this._restClient.GetAsync<OpenWrksBudget>(uri);
            }

            if (string.IsNullOrWhiteSpace(result.Message))
            {
                var mappedResult = _mapper.Map<IncomeAndExpenditure>(result);
                mappedResult.BudgetSource = "MyBudget Tool";

                return mappedResult;
            }

            return null;
        }
     

        private async Task<OpenWrksBudget> GetBudget(string caseflowUserId)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var fullFilePath = $"{_openWrksSetting.TestDataFolder}\\OpenBanking_{caseflowUserId}.json";

            if (!File.Exists(fullFilePath))
            {
                return new OpenWrksBudget { Message = "Customer not found." };
            }

            var content = await File.ReadAllTextAsync(fullFilePath);

            return JsonConvert.DeserializeObject<OpenWrksBudget>(content, settings);
        }
    }
}
