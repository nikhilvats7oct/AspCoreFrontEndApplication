using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Models
{
    public class ResendActivationLinkResult
    {
        public string Message { get; set; }

        public bool IsSuccess { get; set; }
    }
}
