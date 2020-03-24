using System.ComponentModel.DataAnnotations;

namespace FinancialPortal.Web.ViewModels.Base
{
    public class IncomeSourceVm
    {
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public virtual decimal Amount { get; set; }
        public virtual string Frequency { get; set; }
    }
}
