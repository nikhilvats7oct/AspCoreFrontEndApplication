using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Services.Interfaces
{
    public interface IIncomeSource
    {
        decimal Amount { get; set; }
        string Frequency { get; set; }
    }
}
