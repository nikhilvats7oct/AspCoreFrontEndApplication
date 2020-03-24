using System.Collections.Generic;
using FinancialPortal.Web.Services.ApiModels;
using FinancialPortal.Web.Services.Models;

namespace FinancialPortal.Web.Maps
{
    public interface IMapperHelper
    {
        string AppendNonNullWithSpace(string stringBefore, string stringToAdd, string inBetweenText = "");
        string DeriveDiscountDescription(Account account);
        string DerivePlanDescription(Account account);
        string DeriveArrearsDetail(decimal? paymentPlanArrearsAmount, bool paymentPlanIsAutomated);
        string DerivePlanDescription(AccountSummary account);
        string DeriveDiscountDescription(AccountSummary account);
        bool AccountHasDiscountOffer(AccountSummary account);
        string DeriveArrearsSummary(decimal? paymentPlanArrearsAmount);
        string DerivePlanTransferredFromMessage(List<string> accountNumbers);
        string DerivePlanTransferredFromAccountsFormatted(List<string> accountNumbers);
        string ConvertEmploymentStatusFromCaseflow(string employmentStatus);
        string ConvertHousingStatusFromCaseflow(string housingStatus);
        string ConvertFrequencyFromCaseflow(string frequency);
        RegularPayment MapRegularPayment(decimal amount, string frequency);
        Outgoing MapOutgoing(decimal amount, string frequency, decimal arrearsAmount);        
        string ConvertFrequencyToInitial(string initial);
        string ConvertToHousingStatusCaseflow(string status);
        string ConvertToEmploymentStatusCaseflow(string status);
        List<SaveOtherDebtsApiModel> CreateOtherDebts(IncomeAndExpenditure iAndE);
    }
}
