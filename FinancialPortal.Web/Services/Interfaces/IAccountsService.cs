using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinancialPortal.Web.Services.Models;

namespace FinancialPortal.Web.Services.Interfaces
{
    public interface IAccountsService
    {
        Task<List<AccountSummary>> GetAccounts(string userId);
        Task<Account> GetAccount(string userId, string lowellRef);
        Task<CustomerSummary> CreateCustomerSummary(List<AccountSummary> accountSummaries, string leadLowellRef,
                    IDictionary<string, Guid> surrogateKeysByLowellReference);

        Task<List<AccountSummary>> GetMyAccountsSummary(string lowellReference);
    }
}
