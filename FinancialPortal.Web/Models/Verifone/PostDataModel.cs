namespace FinancialPortal.Web.Models.Verifone
{
    public class PostDataModel
    {
        //these fields must match {} placeholders in templates
        public string api { get; set; }
        public string merchantid { get; set; }
        public string systemid { get; set; }
        public string requesttype { get; set; }
        public string requestdata { get; set; } //eft or token reg
    }
}
