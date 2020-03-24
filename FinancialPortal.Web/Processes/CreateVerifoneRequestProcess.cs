using System;
using System.Diagnostics;
using FinancialPortal.Web.Models.Verifone;
using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.Settings;
using FinancialPortal.Web.ViewModels;
using Microsoft.Extensions.Logging;

namespace FinancialPortal.Web.Processes
{
    public class CreateVerifoneRequestProcess : ICreateVerifoneRequestProcess
    {
        private readonly ILogger<CreateVerifoneRequestProcess> _logger;
        private readonly VerifoneSetting _verifoneSetting;

        public CreateVerifoneRequestProcess(ILogger<CreateVerifoneRequestProcess> logger, VerifoneSetting verifoneSetting)
        {
            _logger = logger;
            _verifoneSetting = verifoneSetting;
        }

        public RequestDataModel CreateDataModel(PaymentOptionsVm accountVm)
        {
            var requestDataModel = new RequestDataModel();

            decimal? amount;
            if (accountVm.SelectedPaymentOption == PaymentOptionsSelectionsVm.Values.FullPayment)
                amount = accountVm.FullPaymentAmountDerived;
            else
                amount = accountVm.PartialPaymentAmount;

            // Should never be null, as validated with Fluent
            Debug.Assert(amount != null, "amount != null");

            requestDataModel.api = _verifoneSetting.ApiVersion;
            requestDataModel.merchantid = _verifoneSetting.Merchant.MerchantId;
            requestDataModel.systemid = _verifoneSetting.Merchant.SystemId;
            requestDataModel.systemguid = _verifoneSetting.Merchant.SystemGuid;
            requestDataModel.merchantreference = accountVm.VerifoneTransactionGuid.ToString();
            requestDataModel.returnurlxml = $"<returnurl>{_verifoneSetting.CompletionUrlOverride}</returnurl>";
            requestDataModel.merchanttemplateid = _verifoneSetting.Template.MerchantTemplateId.SetupPaymentPlan;
            requestDataModel.languagetemplateid = 1;
            requestDataModel.capturemethod = 12;
            requestDataModel.processingidentifier = 1;
            requestDataModel.accountid = _verifoneSetting.Merchant.AccountId;
            requestDataModel.transactionvalue = amount.Value;
            requestDataModel.tokenidxml = "";
            requestDataModel.registertoken = "false";
            requestDataModel.tokenexpirationdate = DateTime.Now.AddYears(4).ToString("ddMMyyyy");
            requestDataModel.allowedpaymentschemes = _verifoneSetting.AllowedPaymentSchemes;
            requestDataModel.allowedpaymentmethods = "1";
            requestDataModel.description = "Payment to Lowell account " + accountVm.LowellReference;
            requestDataModel.firstname = "";
            requestDataModel.lastname = "";
            requestDataModel.email = "";
            requestDataModel.address1 = "";
            requestDataModel.address2 = "";
            requestDataModel.postcode = "";
            requestDataModel.town = "";
            requestDataModel.totalamount = accountVm.OutstandingBalance;
            requestDataModel.productname = "Payment to Lowell account " + accountVm.LowellReference;
            requestDataModel.productcode = accountVm.LowellReference;
            requestDataModel.processCPC = false;
            requestDataModel.payerauthxml = "";

            return requestDataModel;
        }
    }
}
