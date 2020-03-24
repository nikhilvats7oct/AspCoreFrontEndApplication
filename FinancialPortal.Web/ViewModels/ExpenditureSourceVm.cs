using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialPortal.Web.ViewModels.Interfaces;

namespace FinancialPortal.Web.ViewModels
{
    public class ExpenditureSourceVm: IExpenditureSource
    {
        public virtual decimal Amount { get; set; }
        public virtual string Frequency { get; set; }
    }
}
