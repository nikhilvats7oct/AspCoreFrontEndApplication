using System;
using System.Threading.Tasks;
using FinancialPortal.Web.DataTransferObjects;
using FinancialPortal.Web.Models;
using FinancialPortal.Web.Proxy.Interfaces;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.ViewModels;
using Microsoft.Extensions.Logging;

namespace FinancialPortal.Web.Services
{
    public class CallbackService : ICallbackService
    {
        private readonly ILogger<CallbackService> _logger;

        private readonly IApiGatewayProxy _apiGateway;

        public CallbackService(ILogger<CallbackService> logger, IApiGatewayProxy apiGateway)
        {
            _logger = logger;
            _apiGateway = apiGateway;
        }

        public Task<ResultDto> SendCallbackMessage(CallbackVm viewModel)
        {
            var callbackDetailDto = new CallbackDetailsDto()
            {
                LowellReferenceNumber = viewModel.LowellReferenceNumber,
                AccountHolderStatus = viewModel.AccountHolderStatus,
                FullName = viewModel.FullName,
                PreferredTelephoneNumber = viewModel.PreferredTelephoneNumber,
                CallmeNow = viewModel.CallmeNow == "CallMeNow",
                CallbackDate = viewModel.CallbackDate != null ? viewModel.CallbackDate.Value : DateTime.MinValue,
                TimeSlot = viewModel.TimeSlot
            };

            return _apiGateway.SendCallbackMessage(callbackDetailDto);
        }
    }
}
