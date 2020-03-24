using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FinancialPortal.Web.Services.ApiModels;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Services.Interfaces.Utility;
using FinancialPortal.Web.Services.Models;
using FinancialPortal.Web.Settings;

namespace FinancialPortal.Web.Services
{
    public class TransactionsService : ITransactionsService
    {    
        private readonly IAccountsService _accountsService;
        private readonly PortalSetting _portalSettings;
        private readonly IRestClient _restClient;
        private readonly IMapper _mapper;

        private IApplicationSessionState session;

        public TransactionsService(IAccountsService accountsService,
                                   PortalSetting portalSettings,
                                   IRestClient restClient,
                                   IMapper mapper)
        {
            _accountsService = accountsService;
            _portalSettings = portalSettings;
            _restClient = restClient;
            _mapper = mapper;
        }

        public async Task<List<Transaction>> GetTransactions(string lowellRef)
        {            
            var url = $"{_portalSettings.GatewayEndpoint}api/ViewTransactions/GetTransactions";

            ApiAccountReferenceModel accountRef = new ApiAccountReferenceModel()
            {
                AccountReference = lowellRef
            };

            ApiTransactionsWrapper result = await _restClient.PostAsync<ApiAccountReferenceModel, ApiTransactionsWrapper>(url, accountRef);
            if (result != null)
            {
                return _mapper.Map<List<ApiTransaction>, List<Transaction>>(result.Payments);
            }

            return null;
        }

    }
}
