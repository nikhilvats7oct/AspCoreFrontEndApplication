using System;
using System.Collections.Generic;
using System.Globalization;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Models.Interfaces;

namespace FinancialPortal.Web.ViewModels
{
    public class DataProtectionVm : IGtmEventRaisingVm
    {
        public DataProtectionVm()
        {
            Days = new List<KeyValuePair<int?, string>>();
            Months = new List<KeyValuePair<string, string>>();
            Years = new List<KeyValuePair<int?, string>>();
        }

        public string LowellReference { get; set; }
        public int? Day { get; set; }
        public string Month { get; set; }
        public int? Year { get; set; }
        public string Postcode { get; set; }

        public DateTime? DateOfBirth
        {
            get
            {
                // Not making any assumptions about server configuration (culture)
                if (DateTime.TryParse($"{Day}/{Month}/{Year}", new CultureInfo("en-GB"), DateTimeStyles.None,
                    out var dt))
                {
                    return dt;
                }

                return null;
            }
        }

        public List<KeyValuePair<int?, string>> Days { get; set; }
        public List<KeyValuePair<string, string>> Months { get; set; }
        public List<KeyValuePair<int?, string>> Years { get; set; }
        public string HoneyPotTextBox { get; set; }
        public string NotificationMessage { get; set; }
        public List<GtmEvent> GtmEvents { get; set; } = new List<GtmEvent>();
    }
}