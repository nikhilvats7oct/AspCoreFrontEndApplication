using System;
using FinancialPortal.Web.Models.Verifone;
using FinancialPortal.Web.Processes;
using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.Settings;
using FinancialPortal.Web.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FinancialPortal.UnitTests.Processes
{
    [TestClass]
    public class CreateVerifoneRequestProcessTests
    {
        private Mock<ILogger<CreateVerifoneRequestProcess>> _loggerMock;

        private ICreateVerifoneRequestProcess _createVerifoneRequestProcess;

        [TestInitialize]
        public void Initialise()
        {
            var verifoneSetting = new VerifoneSetting
            {
                Merchant = new MerchantSetting { },
                Template = new TemplateSetting
                    { MerchantTemplateId = new MerchantTemplateSetting {  } }
            };

            _loggerMock = new Mock<ILogger<CreateVerifoneRequestProcess>>();

            _createVerifoneRequestProcess = new CreateVerifoneRequestProcess(_loggerMock.Object, verifoneSetting);
        }

        [TestMethod]
        public void CreateDataModel_WhenSelectedPaymentOptionIsFull_TransactionValueIsFullPaymentAmountDerived()
        {
            PaymentOptionsVm paymentOptionsVm = new PaymentOptionsVm()
            {
                SelectedPaymentOption = PaymentOptionsSelectionsVm.Values.FullPayment,
                VerifoneTransactionGuid = "fb3ce06f-af36-40d4-a88b-dab5f39b0893",
                FullPaymentBalance = 123.45m,
                LowellReference = "anyoldref",
                OutstandingBalance = 211.12m
            };

            RequestDataModel requestDataModel = _createVerifoneRequestProcess.CreateDataModel(paymentOptionsVm);

            string tokenexpirationdate = DateTime.Now.AddYears(4).ToString("ddMMyyyy");

            Assert.AreEqual(null, requestDataModel.api); // ConfigurationManager.AppSettings["Verifone:PostData:Api"]
            Assert.AreEqual(null, requestDataModel.merchantid); // ConfigurationManager.AppSettings["Verifone:Merchant:MerchantId"]
            Assert.AreEqual(null, requestDataModel.systemid); // ConfigurationManager.AppSettings["Verifone:Merchant:SystemId"]
            Assert.AreEqual(null, requestDataModel.systemguid); // ConfigurationManager.AppSettings["Verifone:Merchant:SystemGuid"])
            Assert.AreEqual("fb3ce06f-af36-40d4-a88b-dab5f39b0893", requestDataModel.merchantreference);
            Assert.AreEqual("<returnurl></returnurl>", requestDataModel.returnurlxml); // $"<returnurl>{ConfigurationManager.AppSettings["Verifone:Url:CompletionUrlOverride"]}</returnurl>";
            Assert.AreEqual(null, requestDataModel.merchanttemplateid); // ConfigurationManager.AppSettings["Verifone:Template:MerchantTemplateId:SetupPaymentPlan"]
            Assert.AreEqual(1, requestDataModel.languagetemplateid);
            Assert.AreEqual(12, requestDataModel.capturemethod);
            Assert.AreEqual(1, requestDataModel.processingidentifier);
            Assert.AreEqual(null, requestDataModel.accountid); // ConfigurationManager.AppSettings["Verifone:Merchant:AccountId"]
            Assert.AreEqual(123.45m, requestDataModel.transactionvalue);
            Assert.AreEqual("", requestDataModel.tokenidxml);
            Assert.AreEqual("false", requestDataModel.registertoken);
            Assert.AreEqual(tokenexpirationdate, requestDataModel.tokenexpirationdate);
            Assert.AreEqual(null, requestDataModel.allowedpaymentschemes); // ConfigurationManager.AppSettings["Verifone:PostData:AllowedPaymentSchemes"]
            Assert.AreEqual("1", requestDataModel.allowedpaymentmethods);
            Assert.AreEqual("Payment to Lowell account anyoldref", requestDataModel.description);
            Assert.AreEqual("", requestDataModel.firstname);
            Assert.AreEqual("", requestDataModel.lastname);
            Assert.AreEqual("", requestDataModel.email);
            Assert.AreEqual("", requestDataModel.address1);
            Assert.AreEqual("", requestDataModel.address2);
            Assert.AreEqual("", requestDataModel.postcode);
            Assert.AreEqual("", requestDataModel.town);
            Assert.AreEqual(211.12m, requestDataModel.totalamount);
            Assert.AreEqual("Payment to Lowell account anyoldref", requestDataModel.productname);
            Assert.AreEqual("anyoldref", requestDataModel.productcode);
            Assert.AreEqual(false, requestDataModel.processCPC);
            Assert.AreEqual("", requestDataModel.payerauthxml);

            Assert.Inconclusive("Unable to unit test completely. Should be taking IApplicationSettingsReader as a method parameter, so as to be able to mock config.");

            Assert.Inconclusive("Make internal method that takes token expiration date, so that it can be unit tested in detail.");
        }

        [TestMethod]
        public void CreateDataModel_WhenSelectedPaymentOptionNotFull_TransactionValueIsPartialPaymentAmount()
        {
            PaymentOptionsVm paymentOptionsVm = new PaymentOptionsVm()
            {
                SelectedPaymentOption = PaymentOptionsSelectionsVm.Values.PartialPayment,
                VerifoneTransactionGuid = "fb3ce06f-af36-40d4-a88b-dab5f39b0893",
                PartialPaymentAmount = 987.65m,
                LowellReference = "anyoldref",
                OutstandingBalance = 211.12m
            };

            RequestDataModel requestDataModel = _createVerifoneRequestProcess.CreateDataModel(paymentOptionsVm);

            string tokenexpirationdate = DateTime.Now.AddYears(4).ToString("ddMMyyyy");

            Assert.AreEqual(null, requestDataModel.api); // ConfigurationManager.AppSettings["Verifone:PostData:Api"]
            Assert.AreEqual(null, requestDataModel.merchantid); // ConfigurationManager.AppSettings["Verifone:Merchant:MerchantId"]
            Assert.AreEqual(null, requestDataModel.systemid); // ConfigurationManager.AppSettings["Verifone:Merchant:SystemId"]
            Assert.AreEqual(null, requestDataModel.systemguid); // ConfigurationManager.AppSettings["Verifone:Merchant:SystemGuid"])
            Assert.AreEqual("fb3ce06f-af36-40d4-a88b-dab5f39b0893", requestDataModel.merchantreference);
            Assert.AreEqual("<returnurl></returnurl>", requestDataModel.returnurlxml); // $"<returnurl>{ConfigurationManager.AppSettings["Verifone:Url:CompletionUrlOverride"]}</returnurl>";
            Assert.AreEqual(null, requestDataModel.merchanttemplateid); // ConfigurationManager.AppSettings["Verifone:Template:MerchantTemplateId:SetupPaymentPlan"]
            Assert.AreEqual(1, requestDataModel.languagetemplateid);
            Assert.AreEqual(12, requestDataModel.capturemethod);
            Assert.AreEqual(1, requestDataModel.processingidentifier);
            Assert.AreEqual(null, requestDataModel.accountid); // ConfigurationManager.AppSettings["Verifone:Merchant:AccountId"]
            Assert.AreEqual(987.65m, requestDataModel.transactionvalue);
            Assert.AreEqual("", requestDataModel.tokenidxml);
            Assert.AreEqual("false", requestDataModel.registertoken);
            Assert.AreEqual(tokenexpirationdate, requestDataModel.tokenexpirationdate);
            Assert.AreEqual(null, requestDataModel.allowedpaymentschemes); // ConfigurationManager.AppSettings["Verifone:PostData:AllowedPaymentSchemes"]
            Assert.AreEqual("1", requestDataModel.allowedpaymentmethods);
            Assert.AreEqual("Payment to Lowell account anyoldref", requestDataModel.description);
            Assert.AreEqual("", requestDataModel.firstname);
            Assert.AreEqual("", requestDataModel.lastname);
            Assert.AreEqual("", requestDataModel.email);
            Assert.AreEqual("", requestDataModel.address1);
            Assert.AreEqual("", requestDataModel.address2);
            Assert.AreEqual("", requestDataModel.postcode);
            Assert.AreEqual("", requestDataModel.town);
            Assert.AreEqual(211.12m, requestDataModel.totalamount);
            Assert.AreEqual("Payment to Lowell account anyoldref", requestDataModel.productname);
            Assert.AreEqual("anyoldref", requestDataModel.productcode);
            Assert.AreEqual(false, requestDataModel.processCPC);
            Assert.AreEqual("", requestDataModel.payerauthxml);

            Assert.Inconclusive("Unable to unit test completely. Should be taking IApplicationSettingsReader as a method parameter, so as to be able to mock config.");

            Assert.Inconclusive("Make internal method that takes token expiration date, so that it can be unit tested in detail.");
        }

    }
}