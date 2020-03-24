using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialPortal.Web.Services.Models;

namespace FinancialPortal.Web.Services.Interfaces
{
    public interface IOpenWrksService
    {
        Task<OpenWrksInvitationResponse> SendInvitationRequest(OpenWrksInvitationRequest request);
        Task<IncomeAndExpenditure> GetOpenWorksBudgetTranslatedToCaseflowBudgetModel(string caseflowUserId);
    }
}
