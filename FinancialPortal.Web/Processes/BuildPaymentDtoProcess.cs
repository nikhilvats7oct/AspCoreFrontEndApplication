using System;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.ViewModels;
using Microsoft.Extensions.Logging;

namespace FinancialPortal.Web.Processes
{
    public class BuildPaymentDtoProcess : IBuildPaymentDtoProcess
    {
        private readonly ILogger<BuildPaymentDtoProcess> _logger;

        public BuildPaymentDtoProcess(ILogger<BuildPaymentDtoProcess> logger)
        {
            _logger = logger;
        }

        public PaymentDto BuildPaymentDto(PaymentResultVm paymentResultVm, OneOffPaymentDto oneOffPaymentDto)
        {
            var dto = new PaymentDto()
            {
                PlanInPlace = paymentResultVm.PaymentInfo.PlanInPlace,
                SettlementAmount = paymentResultVm.PaymentInfo.DiscountSelected && paymentResultVm.PaymentInfo.PaidInFull,
                LowellReference = oneOffPaymentDto.LowellReference,
                AuthCode = paymentResultVm.ACode,
                Amount = oneOffPaymentDto.PaymentAmount,
                SourceOfFunds = oneOffPaymentDto.SourceOfFunds + ":" + oneOffPaymentDto.SourceOfFundsOther,
                CardId = "",
                ReplayId = null,
                User = String.IsNullOrEmpty(paymentResultVm.PaymentInfo.UserID) ? "WebAnon" : "WebUser"
            };

            return dto;
        }
    }
}
