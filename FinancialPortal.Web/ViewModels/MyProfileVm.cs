using System;
using System.Collections.Generic;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Models.Interfaces;

namespace FinancialPortal.Web.ViewModels
{
    [Serializable]
    public class MyProfileVm : IGtmEventRaisingVm
    {
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public ChangePasswordVm ChangePasswordVm { get; set; } = new ChangePasswordVm();
        public ChangeEmailVm ChangeEmailVm { get; set; } = new ChangeEmailVm();
        public bool AwaitingChangeEmailConfirmation { get; set; }
        public string LowellReference { get; set; }
        public string LoggedInUserID { get; set; }
        public bool EnableContactPreferences { get; set; }
        public ContactPreferencesVm ContactPreferencesVm { get; set; }
        public List<GtmEvent> GtmEvents { get; set; } = new List<GtmEvent>();
    }
}