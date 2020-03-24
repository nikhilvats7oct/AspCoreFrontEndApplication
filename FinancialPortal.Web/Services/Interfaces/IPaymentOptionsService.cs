using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Services.Interfaces
{
    public interface IPaymentOptionsService
    {
        Task<PaymentOptionsVm> GetPaymentOptionsVmForLoggedOnUser(
            string lowellRef);

        Task<PaymentOptionsDto> GetPaymentOptions(string lowellRef);
       
        ////Task<PaymentOptionsVm> GetPaymentOptionsVmForAnonymousUser(
        ////    string lowellRef);
    }
}
