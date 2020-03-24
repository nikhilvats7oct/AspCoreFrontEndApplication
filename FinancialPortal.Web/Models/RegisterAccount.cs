using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Models
{
    public class RegisterAccount
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public string LowellReferenceNumber { get; set; }
    }
}
