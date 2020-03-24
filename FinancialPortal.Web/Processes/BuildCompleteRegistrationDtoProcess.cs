using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Processes.Interfaces;
using Microsoft.Extensions.Logging;

namespace FinancialPortal.Web.Processes
{
    public class BuildCompleteRegistrationDtoProcess : IBuildCompleteRegistrationDtoProcess
    {
        private readonly ILogger<BuildCompleteRegistrationDtoProcess> _logger;

        public BuildCompleteRegistrationDtoProcess(ILogger<BuildCompleteRegistrationDtoProcess> logger)
        {
            _logger = logger;
        }

        public CompleteRegistrationDto Build(string lowellReference, string userId, string userEmail)
        {
            var dto = new CompleteRegistrationDto()
            {
                EmailAddress = userEmail,
                LowellReference = lowellReference,
                UserId = userId
            };
            return dto;
        }
    }
}
