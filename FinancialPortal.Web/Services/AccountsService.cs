using AutoMapper;
using FinancialPortal.Web.Services.ApiModels;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Services.Models;
using FinancialPortal.Web.Settings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Services
{
    public class AccountsService : IAccountsService
    {
        private readonly IMapper _mapper;
        private readonly PortalSetting _settings;
        private readonly IRestClient _restClient;
        private readonly IBudgetCalculatorService _budgetCalculatorService;

        public AccountsService(PortalSetting settings,
                               IRestClient restClient,
                               IMapper mapper,
                               IBudgetCalculatorService budgetCalculatorService)
        {
            _settings = settings;
            _restClient = restClient;
            _mapper = mapper;
            _budgetCalculatorService = budgetCalculatorService;
        }

        public async Task<List<AccountSummary>> GetAccounts(string userId)
        {
            ApiUserIdModel userIdModel = new ApiUserIdModel() { UserId = userId };
            string url = $"{_settings.GatewayEndpoint}api/MyAccounts/GetMyAccounts";

            ApiAccountSummaries apiSummary = await _restClient.PostAsync<ApiUserIdModel, ApiAccountSummaries>(url, userIdModel);
            AccountSummaries serviceSummary = _mapper.Map<ApiAccountSummaries, AccountSummaries>(apiSummary);

            return serviceSummary.Accounts;
        }

        public async Task<Account> GetAccount(string userId, string lowellRef)
        {
            AccountDetailApiRequest request = new AccountDetailApiRequest()
            {
                UserId = userId,
                LowellReference = lowellRef
            };

            string url = $"{_settings.GatewayEndpoint}api/MyAccounts/GetMyAccountsDetail";

            ApiAccount apiAccount = await _restClient.PostAsync<AccountDetailApiRequest, ApiAccount>(url, request);
            Account serviceAccount = _mapper.Map<ApiAccount, Account>(apiAccount);

            return serviceAccount;
        }

        public async Task<CustomerSummary> CreateCustomerSummary(List<AccountSummary> accountSummaries, string leadLowellRef,
            IDictionary<string, Guid> surrogateKeysByLowellReference)
        {
            IncomeAndExpenditure incomeAndExpenditure = null;
            if (!String.IsNullOrEmpty(leadLowellRef))
            {
                incomeAndExpenditure = await _budgetCalculatorService.GetSavedIncomeAndExpenditure(leadLowellRef);
            }

            return new CustomerSummary()
            {
                Accounts = accountSummaries,
                SurrogateKeysByLowellReference = surrogateKeysByLowellReference,
                IncomeAndExpenditure = incomeAndExpenditure
            };
        }


        public async Task<List<AccountSummary>> GetMyAccountsSummary(string lowellReference)
        {
            var request = new ApiAccountIdModel()
            {
                AccountId = lowellReference
            };

            var url = $"{_settings.GatewayEndpoint}api/MyAccounts/GetMyAccountsSummary";

            ApiAccountSummaries apiAccount = await _restClient.PostAsync<ApiAccountIdModel, ApiAccountSummaries>(url, request);
            AccountSummaries accountSummaries = _mapper.Map<ApiAccountSummaries, AccountSummaries>(apiAccount);

            return accountSummaries.Accounts;
        }
    }
}
