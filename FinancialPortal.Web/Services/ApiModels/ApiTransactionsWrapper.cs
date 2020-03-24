using System.Collections.Generic;

namespace FinancialPortal.Web.Services.ApiModels
{
    public class ApiTransactionsWrapper
    {
        public List<ApiTransaction> Payments { get; set; }
    }
}
