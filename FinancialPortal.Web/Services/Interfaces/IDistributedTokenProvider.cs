using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Services.Interfaces
{
    public interface IDistributedTokenProvider
    {
        Task<string> GetDistributedTokenId();
    }
}
