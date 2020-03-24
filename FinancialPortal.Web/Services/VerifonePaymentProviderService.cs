using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using FinancialPortal.Web.Extensions;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.Proxy.Interfaces;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.ViewModels;
using Microsoft.Extensions.Logging;

namespace FinancialPortal.Web.Services
{
    public class VerifonePaymentProviderService : IVerifonePaymentProviderService
    {
        private readonly IApiGatewayProxy _apiGatewayProxy;
        private readonly ICreateVerifonePostProcess _createVerifonePostProcess;
        private readonly ICreateVerifoneRequestProcess _createVerifoneRequestProcess;

        private readonly ILogger<VerifonePaymentProviderService> _logger;
        private bool _disposed;
        private string _postdataTemplateXml;
        private string _requestTemplateXml;

        public VerifonePaymentProviderService(ILogger<VerifonePaymentProviderService> logger,
            ICreateVerifoneRequestProcess createVerifoneRequestProcess,
            ICreateVerifonePostProcess createVerifonePostProcess,
            IApiGatewayProxy apiGatewayProxy)
        {
            _logger = logger;
            _createVerifoneRequestProcess = createVerifoneRequestProcess;
            _createVerifonePostProcess = createVerifonePostProcess;
            _apiGatewayProxy = apiGatewayProxy;
        }

        public string CreatePayload(PaymentOptionsVm accountVm)
        {
            using (var sr = new StreamReader("Verifone/PostData-template.txt"))
            {
                _postdataTemplateXml = sr.ReadToEnd();
            }

            using (var sr = new StreamReader("Verifone/EftRequest-template.txt"))
            {
                _requestTemplateXml = sr.ReadToEnd();
            }

            var requestDataModel = _createVerifoneRequestProcess.CreateDataModel(accountVm);

            var formattedRequestXml = _requestTemplateXml.FormatWith(requestDataModel);

            var htmlEncodedRequest = HttpUtility.HtmlEncode(formattedRequestXml);

            var postDataModel = _createVerifonePostProcess.CreateDataModel(htmlEncodedRequest);

            var formattedPostDataXml = _postdataTemplateXml.FormatWith(postDataModel);

            return formattedPostDataXml;
        }

        public async Task AddVerifoneTransactionAsync(VerifoneTransactionDto verifoneTransactionDto)
        {
            await _apiGatewayProxy.AddVerifoneTransactionAsync(verifoneTransactionDto);
        }

        public async Task<VerifoneTransactionDto> GetVerifoneTransactionAsync(
            string transactionGuid)
        {
            return await _apiGatewayProxy.GetVerifoneTransactionAsync(new VerifoneTransactionDto() { TransactionGuid = transactionGuid });
        }

        public async Task UpdateVerifoneTransactionAsync(VerifoneTransactionDto verifoneTransactionDto)
        {
            await _apiGatewayProxy.UpdateVerifoneTransactionAsync(verifoneTransactionDto);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
            }

            _disposed = true;
        }
    }
}