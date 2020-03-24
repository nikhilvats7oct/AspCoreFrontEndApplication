using FinancialPortal.Web.Services.Models;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Services.Interfaces
{
    public interface ICalculatorService
    {
        MonthlyIncome CalculateMonthlyIncome(IncomeAndExpenditure iAndE);
        MonthlyOutgoings CalculateMonthlyOutgoings(IncomeAndExpenditure iAndE);
        decimal CalculateDisposableIncome(decimal income, decimal expenditure);
        decimal ConvertToMonthly(decimal amount, string frequency);
        decimal CalculateMonthlyBillsAndOutgoings(IncomeAndExpenditure iAndE);
        decimal CalculateMonthlyExpenditure(IncomeAndExpenditure iAndE);

        decimal CalculateOtherDebts(IncomeAndExpenditure iAndE);
        bool InArrears(IncomeAndExpenditure iAndE);
    }
}
