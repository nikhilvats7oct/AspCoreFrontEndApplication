namespace FinancialPortal.Web.Models.Verifone
{
    public class RequestDataModel
    {
        //these fields must match {} placeholders in templates
        public string api { get; set; }
        public string merchantid { get; set; }
        public string systemid { get; set; }
        public string systemguid { get; set; }
        public string merchantreference { get; set; }
        public string returnurlxml { get; set; }
        public string merchanttemplateid { get; set; }
        public int languagetemplateid { get; set; }
        public int capturemethod { get; set; }
        public int processingidentifier { get; set; }
        public string accountid { get; set; }
        public decimal transactionvalue { get; set; }
        public string tokenidxml { get; set; }
        public string registertoken { get; set; }
        public string tokenexpirationdate { get; set; }
        public string allowedpaymentschemes { get; set; }
        public string allowedpaymentmethods { get; set; }
        public string description { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string postcode { get; set; }
        public decimal totalamount { get; set; }
        public string productname { get; set; }
        public string productcode { get; set; }
        public bool processCPC { get; set; }
        public string payerauthxml { get; set; }
        public string town { get; set; }
    }
}
