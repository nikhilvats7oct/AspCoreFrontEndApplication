using System;

namespace FinancialPortal.Web.Models
{
    [Serializable]
    public class ResultDto
    {
        public bool IsSuccessful { get; set; }
        public string MessageForUser { get; set; }
    }
}