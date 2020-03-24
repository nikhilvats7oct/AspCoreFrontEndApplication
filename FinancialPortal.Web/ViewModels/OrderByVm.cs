using Microsoft.AspNetCore.Mvc;
using System;

namespace FinancialPortal.Web.ViewModels
{
    public class OrderByVm
    {
        [FromQuery(Name = "orderByReceived")]
        public string OrderByReceived { get; set; }

        [FromQuery(Name = "orderByRead")]
        public string OrderByRead { get; set; }

        [FromQuery(Name = "orderBySubject")]
        public string OrderBySubject { get; set; }

        [FromQuery(Name = "accountReference")]
        public string AccountReference { get; set; }

        [FromQuery(Name = "dateTo")]
        public DateTime? DateTo { get; set; }

        [FromQuery(Name = "dateFrom")]
        public DateTime? DateFrom { get; set; }

        [FromQuery(Name = "keyword")]
        public string Keyword { get; set; }
    }
}
