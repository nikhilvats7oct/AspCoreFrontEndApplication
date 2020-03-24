using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Models
{
    public class RegisterAccountResult
    {
        public Guid SubjectId { get; set; }

        public int ErrorCode { get; set; }

        public string Message { get; set; }

        public bool IsSuccess { get; set; }
    }
}
