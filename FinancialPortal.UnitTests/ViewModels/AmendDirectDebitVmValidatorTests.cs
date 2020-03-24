using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using FinancialPortal.Web.Validation;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.UnitTests.ViewModels
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class AmendDirectDebitVmValidatorTests
    {
        private AmendDirectDebitVmValidator _validator;

        [TestInitialize]
        public void Initialize()
        {
            _validator = new AmendDirectDebitVmValidator();
        }

        [TestMethod]
        public void ValidStateTest()
        {
            AmendDirectDebitVm vm = new AmendDirectDebitVm()
            {
                ClientName = "Test Client",
                EarliestStartDate = DateTime.Now.AddDays(-14),
                LatestStartDate = DateTime.Now.AddDays(180),
                PlanStartDate = DateTime.Now.AddDays(15),
                DirectDebitAmount = 12.34M,
                LowellReference = "12345678",
                PlanType = "Direct Debit",
                PlanFrequency = "Testing...",
                OutstandingBalance = 500,
                SelectedPlanSetupOption = PlanSetupOptions.OtherPaymentOffer
            };

            ValidationResult result = _validator.Validate(vm);

            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(0, result.Errors.Count);
        }

        [TestMethod]
        public void PaymentAmountZero()
        {
            AmendDirectDebitVm vm = new AmendDirectDebitVm()
            {
                ClientName = "Test Client",
                EarliestStartDate = DateTime.Now.AddDays(-14),
                LatestStartDate = DateTime.Now.AddDays(180),
                PlanStartDate = DateTime.Now.AddDays(15),
                DirectDebitAmount = 0.00M,
                LowellReference = "12345678",
                PlanType = "Direct Debit",
                PlanFrequency = "Testing...",
                OutstandingBalance = 500,
                SelectedPlanSetupOption = PlanSetupOptions.OtherPaymentOffer
            };

            ValidationResult result = _validator.Validate(vm);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(ValidationMessages.InvalidAmount, result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void PaymentAmountNegative()
        {
            AmendDirectDebitVm vm = new AmendDirectDebitVm()
            {
                ClientName = "Test Client",
                EarliestStartDate = DateTime.Now.AddDays(-14),
                LatestStartDate = DateTime.Now.AddDays(180),
                PlanStartDate = DateTime.Now.AddDays(15),
                DirectDebitAmount = -12.34M,
                LowellReference = "12345678",
                PlanType = "Direct Debit",
                PlanFrequency = "Testing...",
                OutstandingBalance = 500,
                SelectedPlanSetupOption = PlanSetupOptions.DisposableIncome
            };

            ValidationResult result = _validator.Validate(vm);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(ValidationMessages.InvalidAmount, result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void PaymentAmountGreaterThanBalance()
        {
            AmendDirectDebitVm vm = new AmendDirectDebitVm()
            {
                ClientName = "Test Client",
                EarliestStartDate = DateTime.Now.AddDays(-14),
                LatestStartDate = DateTime.Now.AddDays(180),
                PlanStartDate = DateTime.Now.AddDays(15),
                DirectDebitAmount = 500.50M,
                LowellReference = "12345678",
                PlanType = "Direct Debit",
                PlanFrequency = "Testing...",
                OutstandingBalance = 500,
                SelectedPlanSetupOption = PlanSetupOptions.AverageSetupValue
            };

            ValidationResult result = _validator.Validate(vm);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual($"{ValidationMessages.AmountGreaterThenAllowed}500", result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void PlanFrequencyEmpty()
        {
            AmendDirectDebitVm vm = new AmendDirectDebitVm()
            {
                ClientName = "Test Client",
                EarliestStartDate = DateTime.Now.AddDays(-14),
                LatestStartDate = DateTime.Now.AddDays(180),
                PlanStartDate = DateTime.Now.AddDays(15),
                DirectDebitAmount = 55.50M,
                LowellReference = "12345678",
                PlanType = "Direct Debit",
                PlanFrequency = "",
                OutstandingBalance = 500,
                SelectedPlanSetupOption = PlanSetupOptions.OtherPaymentOffer
            };

            ValidationResult result = _validator.Validate(vm);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(ValidationMessages.NoFrequencySelected, result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void PlanStartDateBeforeEarliest()
        {
            AmendDirectDebitVm vm = new AmendDirectDebitVm()
            {
                ClientName = "Test Client",
                EarliestStartDate = DateTime.Now.AddDays(-14),
                LatestStartDate = DateTime.Now.AddDays(180),
                PlanStartDate = DateTime.Now.AddDays(-15),
                DirectDebitAmount = 55.50M,
                LowellReference = "12345678",
                PlanType = "Direct Debit",
                PlanFrequency = "Testing...",
                OutstandingBalance = 500,
                SelectedPlanSetupOption = PlanSetupOptions.DisposableIncome
            };

            ValidationResult result = _validator.Validate(vm);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(ValidationMessages.SelectedStartDateTooEarly, result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void PlanStartDateAfterLatest()
        {
            AmendDirectDebitVm vm = new AmendDirectDebitVm()
            {
                ClientName = "Test Client",
                EarliestStartDate = DateTime.Now.AddDays(-14),
                LatestStartDate = DateTime.Now.AddDays(180),
                PlanStartDate = DateTime.Now.AddDays(200),
                DirectDebitAmount = 55.50M,
                LowellReference = "12345678",
                PlanType = "Direct Debit",
                PlanFrequency = "Testing...",
                OutstandingBalance = 500,
                SelectedPlanSetupOption = PlanSetupOptions.AverageSetupValue
            };

            ValidationResult result = _validator.Validate(vm);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(ValidationMessages.SelectedStartDateTooLate, result.Errors[0].ErrorMessage);
        }
    }
}
