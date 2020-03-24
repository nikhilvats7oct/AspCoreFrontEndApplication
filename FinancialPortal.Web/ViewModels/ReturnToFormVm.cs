namespace FinancialPortal.Web.ViewModels
{
    public class ReturnToFormVm
    {
        public string State { get; set; }           // appropriate serialised state for action
        public string StateFieldName { get; set; }  // name for form element - so as to map correctly to model received by action
        public string StateType { get; set; }
        public string ReturnControllerName { get; set; }
        public string ReturnActionName { get; set; }
    }
}
