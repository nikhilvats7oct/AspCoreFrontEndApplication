using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialPortal.Web.ViewModels
{
    public interface ICacheableViewModel
    {
        string Key { get; set; }
    }
}
