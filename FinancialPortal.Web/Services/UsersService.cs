using System.Threading.Tasks;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FinancialPortal.Web.Services
{
    public class UsersService : IUsersService
    {
        private readonly ILogger<UsersService> _logger;
        private readonly IGetUserProcess _getUserProcess;

        public UsersService(ILogger<UsersService> logger,
                            IGetUserProcess getUserProcess)
        {
            _logger = logger;
            _getUserProcess = getUserProcess;
        }

        public Task<UserDto> GetUser(string userId)
        {
            return _getUserProcess.GetUser(userId);
        }
    }
}
