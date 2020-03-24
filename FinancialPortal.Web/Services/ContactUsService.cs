using System.Threading.Tasks;
using FinancialPortal.Web.Models;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Proxy.Interfaces;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.ViewModels;
using Microsoft.Extensions.Logging;

namespace FinancialPortal.Web.Services
{
    public class ContactUsService : IContactUsService
    {
        private readonly ILogger<ContactUsService> _logger;

        private readonly IApiGatewayProxy _apiGateway;

        public ContactUsService(ILogger<ContactUsService> logger, IApiGatewayProxy apiGateway)
        {
            _logger = logger;
            _apiGateway = apiGateway;
        }

        public Task<ResultDto> SendContactUsMessage(ContactUsVm viewModel)
        {
            var contactUsDetailDto = new ContactUsDetailsDto()
            {
                LowellReferenceNumber = viewModel.LowellReferenceNumber,
                AccountHolderStatus =  viewModel.AccountHolderStatus,
                QueryTopic =  viewModel.QueryTopic,
                FirstLineOfAddress =  viewModel.FirstLineOfAddress,
                EmailAddress = viewModel.ContactUsEmailAddress,
                DateOfBirth = viewModel.DateOfBirth.Value,
                Postcode = viewModel.Postcode,
                FullName = viewModel.FullName,
                MessageContent = viewModel.MessageContent,
                AuthorisedThirdPartyPassword = viewModel.AuthorisedThirdPartyPassword
            };

           return _apiGateway.SendContactUsMessage(contactUsDetailDto);
        }
    }
}
