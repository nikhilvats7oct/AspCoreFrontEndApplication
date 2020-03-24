using System.Collections.Generic;

namespace FinancialPortal.Web.Models.DataTransferObjects
{
    public class JsonWebToken
    {
        public string iss { get; set; }
        public int exp { get; set; }
        public string client_id { get; set; }
        public List<string> scope { get; set; }
    }
}