namespace FinancialPortal.Web.ViewModels
{
    // Used to display occurrence ID on portal error page
    public class ErrorViewModel
    {
        public bool ShowOccurrenceInformation { get; set; }
        public string ExceptionOccurrenceId { get; set; }
    }
}
