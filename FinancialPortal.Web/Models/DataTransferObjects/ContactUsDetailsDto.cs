using System;

namespace FinancialPortal.Web.Models.DataTransferObjects
{
    public class ContactUsDetailsDto
    {
        public string QueryTopic { get; set; }

        public string AccountHolderStatus { get; set; }

        public string AuthorisedThirdPartyPassword { get; set; }

        public string LowellReferenceNumber { get; set; }

        public string EmailAddress { get; set; }

        public string FullName { get; set; }

        public string FirstLineOfAddress { get; set; }

        public string Postcode { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string MessageContent { get; set; }

        public string ReplayId { get; set; }
    }
}
