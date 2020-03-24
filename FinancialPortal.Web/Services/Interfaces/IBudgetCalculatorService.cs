using FinancialPortal.Web.Services.Models;
using System;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Services.Interfaces
{
    public interface IBudgetCalculatorService
    {
        Task<IncomeAndExpenditure> GetSavedIncomeAndExpenditure(string lowellReference);
        Task<IncomeAndExpenditure> GetPartiallySavedIncomeAndExpenditure(string lowellReference, Guid caseflowUserId);       
        BudgetSummary GetBudgetSummary(IncomeAndExpenditure incomeAndExpenditure, Guid lowellReferenceSurrogateKey, string loggedInUserId);
        Task RemovePartialSaved(Guid caseflowUserId);
        Task SaveIncomeAndExpenditure(IncomeAndExpenditure incomeAndExpenditure, string lowellReference);
        Task<bool> PartiallySaveIncomeAndExpenditure(IncomeAndExpenditure incomeAndExpenditure, string lowellReference, Guid caseflowUserId);
    }
}
