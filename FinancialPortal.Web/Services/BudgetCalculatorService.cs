using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Services.Models;
using System;
using System.Threading.Tasks;
using FinancialPortal.Web.Services.ApiModels;
using AutoMapper;
using FinancialPortal.Web.Settings;

namespace FinancialPortal.Web.Services
{
    public class BudgetCalculatorService : IBudgetCalculatorService
    {
        private readonly ICalculatorService _calculatorService;
        private readonly PortalSetting _portalSettings;
        private readonly IRestClient _restClient;
        private readonly IMapper _mapper;

        public BudgetCalculatorService(
            IRestClient restClient,
            ICalculatorService calculatorService,
            PortalSetting portalSettings,
            IMapper mapper)
        {
            _restClient = restClient;
            _calculatorService = calculatorService;
            _portalSettings = portalSettings;
            _mapper = mapper;
        }

        public async Task<IncomeAndExpenditure> GetSavedIncomeAndExpenditure(string lowellReference)
        {
            var innerUrl = $"{_portalSettings.GatewayEndpoint}api/BudgetCalculator/GetSavedIncomeAndExpenditure";

            var dto = new IncomeAndExpenditureApiRequest
            {
                LowellReference = lowellReference
            };

            IncomeAndExpenditureApiModel result =
                await _restClient.PostAsync<IncomeAndExpenditureApiRequest, IncomeAndExpenditureApiModel>(innerUrl, dto);

            if (result == null || result.Created < DateTime.UtcNow.AddDays(-180)) { return null; }

            return _mapper.Map<IncomeAndExpenditure>(result);
        }

        public async Task<IncomeAndExpenditure> GetPartiallySavedIncomeAndExpenditure(string loggedInUserId, Guid caseflowUserId)
        {
            if (loggedInUserId == null) { return null; }
            if (!_portalSettings.Features.EnablePartialSave) { return null; }

            var innerUrl = $"{_portalSettings.GatewayEndpoint}api/BudgetCalculator/GetPartialSavedIncomeAndExpenditure";

            var dto = new PartialBudgetApiRequest
            {
                CaseflowUserId = caseflowUserId
            };

            IncomeAndExpenditureApiModel result = await _restClient.PostAsync<PartialBudgetApiRequest, IncomeAndExpenditureApiModel>(innerUrl, dto);

            if (result == null || result.Created < DateTime.UtcNow.AddDays(-30)) { return null; }

            return _mapper.Map<IncomeAndExpenditure>(result);
        }

        public BudgetSummary GetBudgetSummary(IncomeAndExpenditure incomeAndExpenditure, Guid lowellReferenceSurrogateKey, string loggedInUserId)
        {
            var monthlyIncome = _calculatorService.CalculateMonthlyIncome(incomeAndExpenditure);
            var monthlyExpenses = _calculatorService.CalculateMonthlyOutgoings(incomeAndExpenditure);
            var disposableIncome = _calculatorService.CalculateDisposableIncome(monthlyIncome.Total, monthlyExpenses.Total);

            var budgetSummary = CreateBudgetSummary(monthlyIncome, monthlyExpenses, disposableIncome, incomeAndExpenditure, string.IsNullOrEmpty(loggedInUserId));

            return budgetSummary;
        }

        private BudgetSummary CreateBudgetSummary(MonthlyIncome monthlyIncome, MonthlyOutgoings monthlyExpenses, decimal disosableIncome, IncomeAndExpenditure iAndE, bool anonUser)
        {
            var budgetSummary = new BudgetSummary();

            budgetSummary.IncomeTotal = monthlyIncome.Total;
            budgetSummary.Salary = monthlyIncome.Salary;
            budgetSummary.Benefits = monthlyIncome.Benefits;
            budgetSummary.Pension = monthlyIncome.Pension;
            budgetSummary.Other = monthlyIncome.Other;
            budgetSummary.TotalExpenditure = monthlyExpenses.Total;
            budgetSummary.HouseholdBills = monthlyExpenses.HouseholdBills;
            budgetSummary.Expenditure = monthlyExpenses.Expenditures;
            budgetSummary.DisposableIncome = disosableIncome;
            budgetSummary.Frequency = "monthly";
            budgetSummary.AnonUser = anonUser;

            if (iAndE != null && (iAndE.RentalArrears > 0 ||
                iAndE.CCJsArrears > 0 ||
                iAndE.ChildMaintenanceArrears > 0 ||
                iAndE.CouncilTaxArrears > 0 ||
                iAndE.CourtFinesArrears > 0 ||
                iAndE.ElectricityArrears > 0 ||
                iAndE.GasArrears > 0 ||
                iAndE.MortgageArrears > 0 ||
                iAndE.OtherUtilitiesArrears > 0 ||
                iAndE.RentArrears > 0 ||
                iAndE.SecuredloansArrears > 0 ||
                iAndE.TvLicenceArrears > 0 ||
                iAndE.WaterArrears > 0))
            {
                budgetSummary.PriorityBillsInArrears = true;
            }

            return budgetSummary;
        }

        public async Task RemovePartialSaved(Guid caseflowUserId)
        {
            var innerUrl = $"{_portalSettings.GatewayEndpoint}api/BudgetCalculator/RemovePartialSavedIncomeAndExpenditure";

            var budget = new PartialBudgetApiRequest
            {
                CaseflowUserId = caseflowUserId
            };

            await _restClient.PostNoResponseAsync(innerUrl, budget);
        }

        public async Task SaveIncomeAndExpenditure(IncomeAndExpenditure incomeAndExpenditure, string lowellReference)
        {
            incomeAndExpenditure.LowellReference = lowellReference;
            var incomeAndExpenditureDto =
                _mapper.Map<IncomeAndExpenditure, IncomeAndExpenditureApiModel>(incomeAndExpenditure);

            var innerUrl = $"{_portalSettings.GatewayEndpoint}api/BudgetCalculator/SaveIncomeAndExpenditure";

            await _restClient.PostNoResponseAsync(innerUrl, incomeAndExpenditureDto);
        }

        public async Task<bool> PartiallySaveIncomeAndExpenditure(IncomeAndExpenditure incomeAndExpenditure, string lowellReference, Guid caseflowUserId)
        {
            incomeAndExpenditure.LowellReference = lowellReference;
            var incomeAndExpenditureDto =
                _mapper.Map<IncomeAndExpenditure, IncomeAndExpenditureApiModel>(incomeAndExpenditure);

            var budget = new PartialBudgetApiModel()
            {
                LowellReference = lowellReference,
                CaseflowUserId = caseflowUserId,
                CreatedDate = DateTime.UtcNow,
                PartialBudget = incomeAndExpenditureDto,
            };

            var innerUrl = $"{_portalSettings.GatewayEndpoint}api/BudgetCalculator/PartialSaveIncomeAndExpenditure";

            return await _restClient.PostAsync<PartialBudgetApiModel, bool>(innerUrl, budget);
        }
       
    }
}
