using FinancialPortal.Web.Models.Verifone;
using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.Settings;
using Microsoft.Extensions.Logging;

namespace FinancialPortal.Web.Processes
{
    public class CreateVerifonePostProcess : ICreateVerifonePostProcess
    {
        private readonly ILogger<CreateVerifonePostProcess> _logger;
        private readonly VerifoneSetting _verifoneSetting;

        public CreateVerifonePostProcess(ILogger<CreateVerifonePostProcess> logger,
            VerifoneSetting verifoneSetting)
        {
            _logger = logger;
            _verifoneSetting = verifoneSetting;
        }

        public PostDataModel CreateDataModel(string request)
        {
            var postDataModel = new PostDataModel();

            postDataModel.api = _verifoneSetting.ApiVersion;
            postDataModel.merchantid = _verifoneSetting.Merchant.MerchantId;
            postDataModel.systemid = _verifoneSetting.Merchant.SystemId;
            postDataModel.requesttype = "eftrequest";
            postDataModel.requestdata = request;

            return postDataModel;
        }
    }
}
