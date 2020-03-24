using System;
using System.Collections.Generic;
using System.Globalization;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Models.Interfaces;

namespace FinancialPortal.Web.ViewModels
{
    public class ContactUsVm : IGtmEventRaisingVm
    {
        public string QueryTopic { get; set; }

        public string AccountHolderStatus { get; set; }

        //[RequiredIf("AccountHolderStatus","Authorised 3rd Party", "Please enter authorised 3rd party password")]
        public string AuthorisedThirdPartyPassword { get; set; }

        public string LowellReferenceNumber { get; set; }

        public string ContactUsEmailAddress { get; set; }

        public string FullName { get; set; }

        public string FirstLineOfAddress { get; set; }

        public string Postcode { get; set; }

        public DateTime? DateOfBirth
        {
            get
            {
                // Not making any assumptions about server configuration (culture)
                if (DateTime.TryParse($"{Day}/{Month}/{Year}", new CultureInfo("en-GB"), DateTimeStyles.None,
                    out DateTime dt))
                    return dt;
                else
                    return null;
            }
        }

        public string MessageContent { get; set; }

        public IEnumerable<string> QueryTopicsSelections { get; set; }

        public IEnumerable<KeyValuePair<string,string>> AccountHolderStatuses { get; set; }

        public bool IsThirdPartyPasswordRequired => string.Equals(this.AccountHolderStatus, "Authorised 3rd Party", StringComparison.OrdinalIgnoreCase);

        public List<KeyValuePair<int?, string>> Days { get; set; }

        public List<KeyValuePair<string, string>> Months { get; set; }

        public List<KeyValuePair<int?, string>> Years { get; set; }

        public int? Day { get; set; }

        public string Month { get; set; }

        public int? Year { get; set; }

        public bool HasMessageBeenSentSuccessfully { get; set; }

        public List<GtmEvent> GtmEvents { get; set; } = new List<GtmEvent>();

        public ContactUsVm()
        {
            Days = new List<KeyValuePair<int?, string>>();
            Months = new List<KeyValuePair<string, string>>();
            Years = new List<KeyValuePair<int?, string>>();
        }
    }
}
