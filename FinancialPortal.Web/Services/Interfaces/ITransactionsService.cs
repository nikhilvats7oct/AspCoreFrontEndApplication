using System.Collections.Generic;
using System.Threading.Tasks;
using FinancialPortal.Web.Services.Models;

namespace FinancialPortal.Web.Services.Interfaces
{
    public interface ITransactionsService
    {
        Task<List<Transaction>> GetTransactions(string lowellRef);
    }
}
