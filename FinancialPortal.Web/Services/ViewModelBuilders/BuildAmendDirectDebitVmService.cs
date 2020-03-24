using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.Proxy.Interfaces;
using FinancialPortal.Web.Services.ApiModels;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Services.Interfaces.Utility;
using FinancialPortal.Web.Services.Models;
using FinancialPortal.Web.Settings;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Services.ViewModelBuilders
{
    public class BuildAmendDirectDebitVmService : IBuildAmendDirectDebitVmService
    {
        private readonly IApiGatewayProxy _apiGatewayProxy;
        private readonly IGetCurrentDirectDebitProcess _getCurrentDirectDebitProcess;
        private readonly IBuildFrequencyListProcess _buildFrequencyListProcess;
        private readonly IDirectDebitFrequencyTranslator _directDebitFrequencyTranslator;
        private readonly PortalSetting _portalSetting;
        private readonly IAccountsService _accountService;

        public BuildAmendDirectDebitVmService(IGetCurrentDirectDebitProcess getCurrentDirectDebitProcess,
                                              IBuildFrequencyListProcess buildFrequencyListProcess,
                                              IDirectDebitFrequencyTranslator directDebitFrequencyTranslator,
                                              PortalSetting portalSetting,
                                              IApiGatewayProxy apiGatewayProxy,
                                              IAccountsService accountService)
        {
            _apiGatewayProxy = apiGatewayProxy;
            _getCurrentDirectDebitProcess = getCurrentDirectDebitProcess;
            _buildFrequencyListProcess = buildFrequencyListProcess;
            _directDebitFrequencyTranslator = directDebitFrequencyTranslator;
            _accountService = accountService;
            _portalSetting = portalSetting;
        }

        public async Task<AmendDirectDebitVm> Build(IApplicationSessionState session, Guid lowellReferenceSurrogateKey,string caseflowUserId)
        {
            string lowellReference = session.GetLowellReferenceFromSurrogate(lowellReferenceSurrogateKey);
            AccountReferenceDto accountReferenceDto = new AccountReferenceDto { LowellReference = lowellReference };
            var currentDirectDebit = await _getCurrentDirectDebitProcess.GetCurrentDirectDebit(accountReferenceDto);
            IncomeAndExpenditureApiModel incomeAndExpenditureDto = await _apiGatewayProxy.GetIncomeAndExpenditure(lowellReference);

            var workingAccounts = 0;

            if (caseflowUserId != null)
            {
                List<AccountSummary> accounts = await _accountService.GetAccounts(caseflowUserId);

                workingAccounts = accounts.Count(a => !a.AccountStatusIsClosed);
            }

            PaymentOptionsDto paymentOptionsDto = await _apiGatewayProxy.GetPaymentOptions(accountReferenceDto);            

            var obj = new AmendDirectDebitVm
            {
                LowellReference = currentDirectDebit.LowellReference,
                ClientName = currentDirectDebit.ClientName,
                OutstandingBalance = currentDirectDebit.OutstandingBalance,
                DirectDebitAmount = null,
                PlanType = currentDirectDebit.PlanType,
                EarliestStartDate = currentDirectDebit.EarliestInstalmentDate,
                LatestStartDate = currentDirectDebit.LatestPlanSetupDate,
                PlanStartDate = currentDirectDebit.EarliestInstalmentDate,
                DiscountedBalance = currentDirectDebit.DiscountedBalance,
                DiscountAmount = currentDirectDebit.DiscountAmount,
                DiscountExpiry = currentDirectDebit.DiscountExpiry,
                DiscountPercentage = currentDirectDebit.DiscountPercentage,
                DiscountedBalancePreviouslyAccepted = paymentOptionsDto.DiscountedBalancePreviouslyAccepted,
                Frequency = _buildFrequencyListProcess.BuildFrequencyList(currentDirectDebit.DirectDebitFrequencies),
                IandENotAvailable = incomeAndExpenditureDto==null,
                IandELessThanOrIs12MonthsOld = (incomeAndExpenditureDto != null && incomeAndExpenditureDto.Created.AddMonths(12).Date >= DateTime.Now.Date),
                AverageMonthlyPayment = _portalSetting.AverageMonthlyPaymentAmount,
                MonthlyDisposableIncome = (incomeAndExpenditureDto == null ? 0 :
                    (incomeAndExpenditureDto.DisposableIncome * (_portalSetting.MonthlyDisposableIncomePlanSetupPercentage / 100)))
            };

            obj.AccountCount = workingAccounts;

            if (workingAccounts > 1)
            {
                obj.MonthlyDisposableIncomePerAccount =
                    obj.MonthlyDisposableIncome / workingAccounts;
            }
            else
            {
                obj.MonthlyDisposableIncomePerAccount = obj.MonthlyDisposableIncome;
            }

            return obj;
        }

        public void UpdateFieldsFromUserEntries(AmendDirectDebitVm amendDirectDebitToUpdate, AmendDirectDebitVm amendDirectDebitUserEntries)
        {
            amendDirectDebitToUpdate.PlanFrequency = _directDebitFrequencyTranslator.TranslateClientScriptCompatibleValueToDescription(amendDirectDebitUserEntries.PlanFrequency);
            amendDirectDebitToUpdate.PlanStartDate = amendDirectDebitUserEntries.PlanStartDate;
            amendDirectDebitToUpdate.DirectDebitAmount = amendDirectDebitUserEntries.DirectDebitAmount;
            amendDirectDebitToUpdate.SelectedPlanSetupOption = amendDirectDebitUserEntries.SelectedPlanSetupOption;
        }

    }
}