using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FinancialPortal.Web.Validation;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.UnitTests.ViewModels
{
    [TestClass]
    public class PaymentOptionsVmTests
    {
        PaymentOptionsVmValidator _validator;
        private PaymentOptionsVm _accountVm;

        [TestInitialize]
        public void Initialise()
        {
            _validator = new PaymentOptionsVmValidator();
            _accountVm = new PaymentOptionsVm()
            {
                OutstandingBalance = 100,
                AcceptTermsAndConditions = true,
                SelectedPaymentOption = PaymentOptionsSelectionsVm.Values.FullPayment,

                DirectDebitStartDateEarliest = new DateTime(2018, 8, 28),
                DirectDebitStartDateLatest = new DateTime(2018, 8, 29),
                DirectDebitSelectedStartDate = "28/08/2018"
            };
        }

        [TestMethod]
        public void SelectedPaymentOption_WhenPleaseSelect_ThenReturnErrorMessage()
        {
            // Arrange
            _accountVm.SelectedPaymentOption = PaymentOptionsSelectionsVm.Values.PleaseSelect;

            // Act
            var result = _validator.Validate(_accountVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Please select a payment type", errors[0].ErrorMessage);
        }

        [TestMethod]
        [DataRow("Please select")]
        [DataRow("")]
        public void FullPaymentSourceOfFunds_WhenSourceOfFundsNotSelected_ThenReturnErrorMessage(string selectedSourceOfFunds)
        {
            //Arrange
            _accountVm.SelectedPaymentOption = PaymentOptionsSelectionsVm.Values.FullPayment;
            _accountVm.FullPaymentSelectedSourceOfFunds = selectedSourceOfFunds;

            //Act
            var result = _validator.Validate(_accountVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("To complete payment please tell us where your funds have come from", errors[0].ErrorMessage);
        }

        [TestMethod]
        public void FullPaymentSourceOfFunds_WhenSourceOfFundsSelectedIsOtherTextBeingTooLong_ThenReturnErrorMessage()
        {
            //Arrange
            _accountVm.SelectedPaymentOption = PaymentOptionsSelectionsVm.Values.FullPayment;
            _accountVm.FullPaymentSelectedSourceOfFunds = "Other";
            _accountVm.FullPaymentSourceOfFundsOtherText = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";

            //Act
            var result = _validator.Validate(_accountVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Enter less than 50 characters", errors[0].ErrorMessage);
        }

        [TestMethod]
        public void FullPaymentSourceOfFunds_WhenSourceOfFundsSelectedIsOtherTextBeingEmpty_ThenReturnErrorMessage()
        {
            //Arrange
            _accountVm.SelectedPaymentOption = PaymentOptionsSelectionsVm.Values.FullPayment;
            _accountVm.FullPaymentSelectedSourceOfFunds = "Other";
            _accountVm.FullPaymentSourceOfFundsOtherText = "";

            //Act
            var result = _validator.Validate(_accountVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("To complete payment please tell us where your funds have come from", errors[0].ErrorMessage);
        }

        [TestMethod]
        public void FullPaymentSourceOfFunds_WhenSourceOfFundsSelectedIsNullTextBeingEmpty_ThenReturnErrorMessage()
        {
            //Arrange
            _accountVm.SelectedPaymentOption = PaymentOptionsSelectionsVm.Values.FullPayment;
            _accountVm.FullPaymentSelectedSourceOfFunds = null;
            _accountVm.FullPaymentSourceOfFundsOtherText = "";

            //Act
            var result = _validator.Validate(_accountVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("To complete payment please tell us where your funds have come from", errors[0].ErrorMessage);
        }

        [TestMethod]
        [DataRow(1.001)]
        [DataRow(1.1111)]
        public void PartialPaymentAmount_WhenMoreThan2DecimalPlaces_ThenReturnErrorMessage(double amount)
        {
            //Arrange
            _accountVm.SelectedPaymentOption = PaymentOptionsSelectionsVm.Values.PartialPayment;
            _accountVm.PartialPaymentAmount = Convert.ToDecimal(amount);
            _accountVm.FullPaymentBalance = Convert.ToDecimal(amount); ;
            _accountVm.PartialPaymentSelectedSourceOfFunds = "Disposable Income";
            _accountVm.DiscountedBalance = 1.99M;
            //Act
            var result = _validator.Validate(_accountVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Please enter a payment amount in pounds and pence.", errors[0].ErrorMessage);
        }

        [TestMethod]
        public void PartialPaymentAmount_WhenNULL_ThenReturnErrorMessage()
        {
            //Arrange
            _accountVm.SelectedPaymentOption = PaymentOptionsSelectionsVm.Values.PartialPayment;
            _accountVm.PartialPaymentAmount = null;
            _accountVm.PartialPaymentSelectedSourceOfFunds = "Disposable Income";

            //Act
            var result = _validator.Validate(_accountVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Please enter a payment amount in pounds and pence.", errors[0].ErrorMessage);
        }

        [TestMethod]
        [DataRow(0.00)]
        [DataRow(-0.01)]
        public void PartialPaymentAmount_WhenLessThanOrEqualToZero_ThenReturnErrorMessage(double? amount)
        {
            //Arrange
            _accountVm.SelectedPaymentOption = PaymentOptionsSelectionsVm.Values.PartialPayment;
            _accountVm.PartialPaymentAmount = (decimal?)amount;
            _accountVm.PartialPaymentSelectedSourceOfFunds = "Disposable Income";

            //Act
            var result = _validator.Validate(_accountVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Amount must be £1.00 or greater", errors[0].ErrorMessage);
        }

        [TestMethod]
        [DataRow(0.99)]
        public void PartialPaymentAmount_WhenLessThanOnePound_ThenReturnErrorMessage(double amount)
        {
            //Arrange
            _accountVm.SelectedPaymentOption = PaymentOptionsSelectionsVm.Values.PartialPayment;
            _accountVm.PartialPaymentAmount = Convert.ToDecimal(amount);
            _accountVm.FullPaymentBalance = Convert.ToDecimal(amount);
            _accountVm.PartialPaymentSelectedSourceOfFunds = "Disposable Income";

            //Act
            var result = _validator.Validate(_accountVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Amount must be £1.00 or greater", errors[0].ErrorMessage);
        }

        [TestMethod]
        [DataRow(1.00)]
        [DataRow(1.01)]
        [DataRow(1.11)]
        public void PartialPaymentAmount_WhenMoreThanOnePound_ThenResultIsValid(double amount)
        {
            //Arrange
            _accountVm.SelectedPaymentOption = PaymentOptionsSelectionsVm.Values.PartialPayment;
            _accountVm.PartialPaymentAmount = Convert.ToDecimal(amount);
            _accountVm.FullPaymentBalance = Convert.ToDecimal(amount);
            _accountVm.PartialPaymentSelectedSourceOfFunds = "Disposable Income";

            //Act
            var result = _validator.Validate(_accountVm);

            //Assert
            Assert.AreEqual(true, result.IsValid);
        }

        [TestMethod]
        [DataRow(50.01, 50.00)]
        public void PartialPaymentAmount_WhenNoDiscountAcceptedAndAmountIsMorethanBalance_ThenReturnErrorMeesage(double amount, double outstandingBalance)
        {
            //Arrange
            _accountVm.SelectedPaymentOption = PaymentOptionsSelectionsVm.Values.PartialPayment;
            _accountVm.DiscountAccepted = false;
            _accountVm.PartialPaymentAmount = Convert.ToDecimal(amount);
            _accountVm.OutstandingBalance = Convert.ToDecimal(outstandingBalance);
            _accountVm.FullPaymentBalance = Convert.ToDecimal(amount);

            _accountVm.PartialPaymentSelectedSourceOfFunds = "Disposable Income";

            //Act
            var result = _validator.Validate(_accountVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual($"You have entered a value greater than allowed. Please enter an amount less than or equal to £{_accountVm.OutstandingBalance}", errors[0].ErrorMessage);
        }

        [TestMethod]
        [DataRow(50.00, 50.00)]
        [DataRow(49.99, 50.00)]
        public void PartialPaymentAmount_WhenDiscountAcceptedAndAmountIsLessThanDiscountedBalance_ThenReturnValid(double amount, double outstandingDiscountBalance)
        {
            //Arrange
            _accountVm.SelectedPaymentOption = PaymentOptionsSelectionsVm.Values.PartialPayment;
            _accountVm.DiscountAccepted = true;
            _accountVm.PartialPaymentAmount = Convert.ToDecimal(amount);
            _accountVm.FullPaymentBalance = Convert.ToDecimal(amount);
            _accountVm.DiscountedBalance = Convert.ToDecimal(outstandingDiscountBalance);
            _accountVm.PartialPaymentSelectedSourceOfFunds = "Disposable Income";

            //Act
            var result = _validator.Validate(_accountVm);

            //Assert
            Assert.AreEqual(true, result.IsValid);
        }

        [TestMethod]
        [DataRow(50.01, 50.00)]
        public void PartialPaymentAmount_WhenDiscountAcceptedAndAmountIsMorethanDiscountedBalance_ThenReturnErrorMeesage(double amount, double outstandingDiscountBalance)
        {
            //Arrange
            _accountVm.SelectedPaymentOption = PaymentOptionsSelectionsVm.Values.PartialPayment;
            _accountVm.DiscountAccepted = true;
            _accountVm.PartialPaymentAmount = Convert.ToDecimal(amount);
            _accountVm.FullPaymentBalance = Convert.ToDecimal(amount);
            _accountVm.DiscountedBalance = Convert.ToDecimal(outstandingDiscountBalance);
            _accountVm.PartialPaymentSelectedSourceOfFunds = "Disposable Income";

            //Act
            var result = _validator.Validate(_accountVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual($"You have entered a value greater than allowed. Please enter an amount less than or equal to £{_accountVm.DiscountedBalance}", errors[0].ErrorMessage);
        }

        [TestMethod]
        [DataRow("Please select")]
        [DataRow("")]
        public void PartialPaymentSourceOfFunds_WhenSourceOfFundsNotSelected_ThenReturnErrorMessage(string selectedSourceOfFunds)
        {
            //Arrange
            _accountVm.SelectedPaymentOption = PaymentOptionsSelectionsVm.Values.PartialPayment;
            _accountVm.PartialPaymentSelectedSourceOfFunds = selectedSourceOfFunds;
            _accountVm.PartialPaymentAmount = 10;
            _accountVm.FullPaymentBalance = 20;

            //Act
            var result = _validator.Validate(_accountVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("To complete payment please tell us where your funds have come from", errors[0].ErrorMessage);
        }

        [TestMethod]
        public void PartialPaymentSourceOfFunds_WhenSourceOfFundsSelectedIsOtherTextBeingTooLong_ThenReturnErrorMessage()
        {
            //Arrange
            _accountVm.SelectedPaymentOption = PaymentOptionsSelectionsVm.Values.PartialPayment;
            _accountVm.PartialPaymentSelectedSourceOfFunds = "Other";
            _accountVm.PartialPaymentSourceOfFundsOtherText = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
            _accountVm.PartialPaymentAmount = 10;
            _accountVm.FullPaymentBalance = 20;

            //Act
            var result = _validator.Validate(_accountVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Enter less than 50 characters", errors[0].ErrorMessage);
        }

        [TestMethod]
        public void PartialPaymentSourceOfFunds_WhenSourceOfFundsSelectedIsOtherTextBeingEmpty_ThenReturnErrorMessage()
        {
            //Arrange
            _accountVm.SelectedPaymentOption = PaymentOptionsSelectionsVm.Values.PartialPayment;
            _accountVm.PartialPaymentSelectedSourceOfFunds = "Other";
            _accountVm.PartialPaymentSourceOfFundsOtherText = "";
            _accountVm.PartialPaymentAmount = 10;
            _accountVm.FullPaymentBalance = 20;

            //Act
            var result = _validator.Validate(_accountVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("To complete payment please tell us where your funds have come from", errors[0].ErrorMessage);
        }

        [TestMethod]
        public void PartialPaymentSourceOfFunds_WhenSourceOfFundsSelectedIsNullTextBeingEmpty_ThenReturnErrorMessage()
        {
            //Arrange
            _accountVm.SelectedPaymentOption = PaymentOptionsSelectionsVm.Values.PartialPayment;
            _accountVm.PartialPaymentSelectedSourceOfFunds = null;
            _accountVm.PartialPaymentSourceOfFundsOtherText = "";
            _accountVm.PartialPaymentAmount = 10;
            _accountVm.FullPaymentBalance = 20;

            //Act
            var result = _validator.Validate(_accountVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("To complete payment please tell us where your funds have come from", errors[0].ErrorMessage);
        }

        [TestMethod]
        public void PartialPaymentAmount_WhenPartialAmountSameAsDiscountedOffer_ThenReturnErrorMessage()
        {
            //Arrange
            _accountVm.SelectedPaymentOption = PaymentOptionsSelectionsVm.Values.PartialPayment;
            _accountVm.DiscountAccepted = false;
            _accountVm.PartialPaymentAmount = Convert.ToDecimal(23.55);
            _accountVm.ProposedDiscountedBalanceIfAccepted = Convert.ToDecimal(23.55);
            _accountVm.FullPaymentBalance = 23.55M;

            _accountVm.PartialPaymentSelectedSourceOfFunds = "Disposable Income";       // required to avoid other validation error

            //Act
            var result = _validator.Validate(_accountVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("The amount entered matches the discount available. If you wish to pay this amount, please click to accept the discount offer. If you do not wish to accept the discounted offer please enter a higher or lower amount.", errors[0].ErrorMessage);
        }

        [TestMethod]

        [DataRow(1, PaymentOptionsSelectionsVm.Values.PartialPayment, false, 10, 9)]        // values different
        [DataRow(2, PaymentOptionsSelectionsVm.Values.PartialPayment, false, 9, 10)]
        [DataRow(3, PaymentOptionsSelectionsVm.Values.PartialPayment, false, 99.51, 99.50)]
        [DataRow(4, PaymentOptionsSelectionsVm.Values.PartialPayment, false, 99.50, 99.51)]

        [DataRow(11, PaymentOptionsSelectionsVm.Values.FullPayment, false, 10, 10)]         // different payment type

        [DataRow(21, PaymentOptionsSelectionsVm.Values.PartialPayment, true, 10, 10)]       // discount accepted

        public void PartialPaymentAmount_WhenPartialAmountNotSameAsDiscountedOffer_OrOtherConditionsNotSatisfied__ThenReturnDoNotErrorMessage(
            int testNumber,
            string testSelectedPaymentOption, bool testDiscountAccepted, double testAmount, double testProposedDiscountedBalanceIfAccepted)
        {
            //Arrange
            _accountVm.SelectedPaymentOption = testSelectedPaymentOption;
            _accountVm.DiscountAccepted = testDiscountAccepted;
            _accountVm.PartialPaymentAmount = Convert.ToDecimal(testAmount);
            _accountVm.ProposedDiscountedBalanceIfAccepted = Convert.ToDecimal(testProposedDiscountedBalanceIfAccepted);
            _accountVm.FullPaymentBalance = (decimal)testAmount;

            // required to avoid other validation errors
            _accountVm.PartialPaymentSelectedSourceOfFunds = "Disposable Income";
            _accountVm.FullPaymentSelectedSourceOfFunds = "Disposable Income";

            // required to avoid failure due to value needing to be <= discounted balance if ticked
            // (discounted balance will equal outstanding balance if balance not accepted)
            _accountVm.DiscountedBalance = _accountVm.OutstandingBalance;

            //Act
            var result = _validator.Validate(_accountVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(0, errors.Count);
        }

        [TestMethod]

        [DataRow(true, true)]       // box ticked, field displayed (anon behaviour) - OK
        [DataRow(false, false)]     // box not ticked, field hidden (logged in behaviour) - OK
        [DataRow(true, false)]      // box ticked, field hidden (should not happen in practice but is valid)

        public void AcceptTermsAndConditions_WhenTrue_OrFieldIsNotDisplayed_ThenNoErrorMessage(
            bool testAcceptTermsAndConditions, bool testIsAcceptTermsAndConditionsFieldVisible)
        {
            //Arrange
            _accountVm.SelectedPaymentOption = PaymentOptionsSelectionsVm.Values.PartialPayment;
            _accountVm.PartialPaymentAmount = 10;
            _accountVm.DiscountedBalance = 9;
            _accountVm.FullPaymentBalance = 20;

            _accountVm.PartialPaymentSelectedSourceOfFunds = "Disposable Income";
            _accountVm.DiscountAccepted = false;

            _accountVm.AcceptTermsAndConditions = testAcceptTermsAndConditions;
            _accountVm.IsAcceptTermsAndConditionsFieldVisible = testIsAcceptTermsAndConditionsFieldVisible;

            //Act
            var result = _validator.Validate(_accountVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(0, errors.Count);
        }

        [TestMethod]
        public void AcceptTermsAndConditions_WhenFalse_AndFieldIsDisplayed_ThenReturnErrorMessage()
        {
            //Arrange
            _accountVm.SelectedPaymentOption = PaymentOptionsSelectionsVm.Values.PartialPayment;
            _accountVm.PartialPaymentAmount = 10;
            _accountVm.DiscountedBalance = 9;
            _accountVm.FullPaymentBalance = 15;

            _accountVm.PartialPaymentSelectedSourceOfFunds = "Disposable Income";
            _accountVm.DiscountAccepted = false;

            _accountVm.AcceptTermsAndConditions = false;
            _accountVm.IsAcceptTermsAndConditionsFieldVisible = true;

            //Act
            var result = _validator.Validate(_accountVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Please accept the terms and conditions.", errors[0].ErrorMessage);
        }

        [TestMethod]
        public void AcceptTermsAndConditions_WhenFalse_AndFieldIsNotDisplayed_ThenSuppressErrorMessage()
        {
            //Arrange
            _accountVm.SelectedPaymentOption = PaymentOptionsSelectionsVm.Values.PartialPayment;
            _accountVm.PartialPaymentAmount = 10;
            _accountVm.DiscountedBalance = 9;
            _accountVm.FullPaymentBalance = 10;

            _accountVm.PartialPaymentSelectedSourceOfFunds = "Disposable Income";
            _accountVm.DiscountAccepted = false;

            _accountVm.AcceptTermsAndConditions = false;
            _accountVm.IsAcceptTermsAndConditionsFieldVisible = false;

            //Act
            var result = _validator.Validate(_accountVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(0, errors.Count);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow(0.00)]
        [DataRow(-0.01)]
        public void DirectDebitAmount_WhenAmountIsEmpty_ThenReturnErrorMessage(double? amount)
        {
            //Arrange
            _accountVm.DirectDebitAmount = (decimal?)amount;
            _accountVm.DirectDebitSelectedFrequency = "W";
            _accountVm.SelectedPaymentOption = PaymentOptionsSelectionsVm.Values.DirectDebit;
            _accountVm.SelectedPlanSetupOption = PlanSetupOptions.DisposableIncome;

            //Act
            var result = _validator.Validate(_accountVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Please enter a payment amount in pounds and pence.", errors[0].ErrorMessage);
        }

        [TestMethod]
        [DataRow(2.00, 2.01)]
        [DataRow(1.00, 1.01)]
        public void DirectDebitAmount_WhenAmountIsMoreThanBalance_ThenReturnErrorMessage(double outstandingBalance, double amount)
        {
            //Arrange
            _accountVm.OutstandingBalance = Convert.ToDecimal(outstandingBalance);
            _accountVm.DirectDebitAmount = Convert.ToDecimal(amount);
            _accountVm.DirectDebitSelectedFrequency = "W";
            _accountVm.SelectedPaymentOption = PaymentOptionsSelectionsVm.Values.DirectDebit;
            _accountVm.DiscountAccepted = false;
            _accountVm.SelectedPlanSetupOption = PlanSetupOptions.DisposableIncome;

            //Act
            var result = _validator.Validate(_accountVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("You have entered a value greater than allowed. Please enter an amount less than or equal to £" + _accountVm.OutstandingBalance, errors[0].ErrorMessage);
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(null)]
        public void DirectDebitFrequency_WhenNoFrequencyOptionSelected_ThenReturnErrorMessage(string frequency)
        {
            //Arrange
            _accountVm.DirectDebitAmount = 1;
            _accountVm.SelectedPaymentOption = PaymentOptionsSelectionsVm.Values.DirectDebit;
            _accountVm.DiscountAccepted = false;
            _accountVm.DirectDebitSelectedFrequency = frequency;
            _accountVm.SelectedPlanSetupOption = PlanSetupOptions.DisposableIncome;

            //Act
            var result = _validator.Validate(_accountVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Please select a payment frequency", errors[0].ErrorMessage);
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(null)]
        public void DirectDebitSelectedStartDate_WhenNoDateEntered_ThenReturnErrorMessage(string selectedStartDate)
        {
            //Arrange
            _accountVm.DirectDebitAmount = 1;
            _accountVm.SelectedPaymentOption = PaymentOptionsSelectionsVm.Values.DirectDebit;
            _accountVm.DiscountAccepted = false;
            _accountVm.DirectDebitSelectedFrequency = "M";
            _accountVm.DirectDebitSelectedStartDate = selectedStartDate;
            _accountVm.SelectedPlanSetupOption = PlanSetupOptions.DisposableIncome;

            //Act
            var result = _validator.Validate(_accountVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Please select a start date", errors[0].ErrorMessage);
        }

        [TestMethod]
        [DataRow("30/08/2018")]
        public void DirectDebitSelectedStartDate_WhenLateDateEntered_ThenReturnErrorMessage(string selectedStartDate)
        {
            //Arrange
            _accountVm.DirectDebitAmount = 1;
            _accountVm.SelectedPaymentOption = PaymentOptionsSelectionsVm.Values.DirectDebit;
            _accountVm.DiscountAccepted = false;
            _accountVm.DirectDebitSelectedFrequency = "M";
            _accountVm.DirectDebitSelectedStartDate = selectedStartDate;
            _accountVm.SelectedPlanSetupOption = PlanSetupOptions.DisposableIncome;

            //Act
            var result = _validator.Validate(_accountVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Please select an earlier date", errors[0].ErrorMessage);

        }

        [TestMethod]
        [DataRow("28/08/2018")]
        [DataRow("29/08/2018")]

        public void DirectDebitSelectedStartDate_WhenValidDateEntered_ThenPassValdiation(string selectedStartDate)
        {
            //Arrange
            _accountVm.DirectDebitAmount = 1;
            _accountVm.SelectedPaymentOption = PaymentOptionsSelectionsVm.Values.DirectDebit;
            _accountVm.DiscountAccepted = false;
            _accountVm.DirectDebitSelectedFrequency = "M";
            _accountVm.DirectDebitSelectedStartDate = selectedStartDate;
            _accountVm.SelectedPlanSetupOption = PlanSetupOptions.DisposableIncome;

            //Act
            var result = _validator.Validate(_accountVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(0, errors.Count);
        }

        [TestMethod]
        [DataRow("27/08/2018")]
        public void DirectDebitSelectedStartDate_WhenEarlyDateEntered_ThenReturnErrorMessage(string selectedStartDate)
        {
            //Arrange
            _accountVm.DirectDebitAmount = 1;
            _accountVm.SelectedPaymentOption = PaymentOptionsSelectionsVm.Values.DirectDebit;
            _accountVm.DiscountAccepted = false;
            _accountVm.DirectDebitSelectedFrequency = "M";
            _accountVm.DirectDebitSelectedStartDate = selectedStartDate;
            _accountVm.SelectedPlanSetupOption = PlanSetupOptions.DisposableIncome;

            //Act
            var result = _validator.Validate(_accountVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Please select a later date", errors[0].ErrorMessage);
        }

        [TestMethod]
        [DataRow(1.001)]
        [DataRow(1.1111)]
        public void DirectDebitAmount_WhenMoreThan2DecimalPlaces_ThenReturnErrorMessage(double amount)
        {
            //Arrange
            _accountVm.DirectDebitAmount = Convert.ToDecimal(amount);
            _accountVm.DirectDebitSelectedFrequency = "W";
            _accountVm.SelectedPaymentOption = PaymentOptionsSelectionsVm.Values.DirectDebit;
            _accountVm.DiscountAccepted = false;
            _accountVm.SelectedPlanSetupOption = PlanSetupOptions.DisposableIncome;

            //Act
            var result = _validator.Validate(_accountVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Please enter a payment amount in pounds and pence.", errors[0].ErrorMessage);
        }

        [TestMethod]
        [DataRow(10, 11)]
        [DataRow(100, 100.01)]
        public void DirectDebit_DiscountedBalanceLessThan0_ThenReturnErrorMessage(double outstandingBalance, double discountAmount)
        {
            //Arrange
            _accountVm.DirectDebitAmount = 10M;
            _accountVm.DirectDebitSelectedFrequency = "W";
            _accountVm.SelectedPaymentOption = PaymentOptionsSelectionsVm.Values.DirectDebit;
            _accountVm.SelectedPlanSetupOption = PlanSetupOptions.DisposableIncome;
            _accountVm.DiscountAccepted = true;
            _accountVm.OutstandingBalance = Convert.ToDecimal(outstandingBalance);
            _accountVm.DiscountAmount = Convert.ToDecimal(discountAmount);
            //Act
            var result = _validator.Validate(_accountVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(ValidationMessages.InvalidDiscountBalance, errors[0].ErrorMessage);
        }

        // Valid - when email field is displayed
        [DataRow("bob@bob.com", true)]
        [DataRow("flob.flobber@gmail.com", true)]

        // Invalid are OK if email is not displayed. This prevents logged in user from being stuck on
        // payments page with no displayed validation message if their associated email is invalid
        // (perhaps due to email validity rules changing or a fault elsewhere in the software)
        // In this case, the CaseFlow sending email stage should fail, rather than blocking the payment/plan
        [DataRow("test@test", false)]
        [DataRow("abc@", false)]
        [DataRow("test@lowell.com", false)]
        [DataRow("", false)]
        [TestMethod]
        public void DirectDebitEmailAddress_WhenEmailIsValid_OrEmailFieldIsNotDisplayed_ThenNoErrorMessage(
            string directDebitEmailAddress, bool directDebitIsEmailAddressFieldVisible)
        {
            //Arrange
            _accountVm.SelectedPaymentOption = PaymentOptionsSelectionsVm.Values.DirectDebit;
            _accountVm.DirectDebitAmount = 5;
            _accountVm.DirectDebitSelectedFrequency = "Weekly";
            _accountVm.OutstandingBalance = 10;
            _accountVm.SelectedPlanSetupOption = PlanSetupOptions.DisposableIncome;
            _accountVm.DirectDebitEmailAddress = directDebitEmailAddress;
            _accountVm.DirectDebitIsEmailAddressFieldVisible = directDebitIsEmailAddressFieldVisible;

            //Act
            var result = _validator.Validate(_accountVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(0, errors.Count);
        }

        [DataRow("test@test")]
        [DataRow("abc@")]
        [DataRow("test@lowell.com")]
        [DataRow("")]
        [TestMethod]
        public void DirectDebitEmailAddress_WhenEmailIsNotValid_AndEmailFieldIsNotDisplayed_ThenSuppressErrorMessage(
            string directDebitEmailAddress)
        {
            //Arrange
            _accountVm.SelectedPaymentOption = PaymentOptionsSelectionsVm.Values.DirectDebit;
            _accountVm.DirectDebitAmount = 5;
            _accountVm.DirectDebitSelectedFrequency = "Weekly";
            _accountVm.OutstandingBalance = 10;
            _accountVm.SelectedPlanSetupOption = PlanSetupOptions.DisposableIncome;
            _accountVm.DirectDebitEmailAddress = directDebitEmailAddress;
            _accountVm.DirectDebitIsEmailAddressFieldVisible = false;

            //Act
            var result = _validator.Validate(_accountVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(0, errors.Count);
        }

        // Full Payment - With Direct Debit Email Address shown (however, would be hidden by JavaScript on UI)
        [DataRow("test@test", PaymentOptionsSelectionsVm.Values.FullPayment, true)]
        [DataRow("abc@", PaymentOptionsSelectionsVm.Values.FullPayment, true)]
        [DataRow("test@lowell.com", PaymentOptionsSelectionsVm.Values.FullPayment, true)]
        [DataRow("", PaymentOptionsSelectionsVm.Values.FullPayment, true)]

        // Partial Payment - With Direct Debit Email Address shown (however, would be hidden by JavaScript on UI)
        [DataRow("test@test", PaymentOptionsSelectionsVm.Values.PartialPayment, true)]
        [DataRow("abc@", PaymentOptionsSelectionsVm.Values.PartialPayment, true)]
        [DataRow("test@lowell.com", PaymentOptionsSelectionsVm.Values.PartialPayment, true)]
        [DataRow("", PaymentOptionsSelectionsVm.Values.PartialPayment, true)]

        // Direct Debit Email Address NOT shown - should still suppress
        [DataRow("abc@", PaymentOptionsSelectionsVm.Values.FullPayment, false)]
        [DataRow("abc@", PaymentOptionsSelectionsVm.Values.PartialPayment, false)]

        [TestMethod]
        public void DirectDebitEmailAddress_WhenEmailIsNotValid_AndNotDirectDebit_ThenSuppressErrorMessage(
            string directDebitEmailAddress, string selectedPaymentOption, bool directDebitIsEmailAddressFieldVisible)
        {
            //Arrange
            _accountVm.SelectedPaymentOption = selectedPaymentOption;

            // Need to populate full and partial fields, to prevent other errors
            _accountVm.FullPaymentSelectedSourceOfFunds = "Disposable Income";
            _accountVm.PartialPaymentAmount = 10;
            _accountVm.FullPaymentBalance = 10;
            _accountVm.PartialPaymentSelectedSourceOfFunds = "Disposable Income";

            _accountVm.DirectDebitEmailAddress = directDebitEmailAddress;
            _accountVm.DirectDebitIsEmailAddressFieldVisible = directDebitIsEmailAddressFieldVisible;

            //Act
            var result = _validator.Validate(_accountVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(0, errors.Count);
        }

        [DataRow("test@test")]
        [DataRow("abc@")]
        [DataRow("test@lowell.com")]
        [DataRow("")]
        [TestMethod]
        public void DirectDebitEmailAddress_WhenEmailIsNotValid_AndEmailFieldIsDisplayed_ThenReturnErrorMessage(
            string directDebitEmailAddress)
        {
            //Arrange
            _accountVm.SelectedPaymentOption = PaymentOptionsSelectionsVm.Values.DirectDebit;
            _accountVm.DirectDebitAmount = 5;
            _accountVm.DirectDebitSelectedFrequency = "Weekly";
            _accountVm.OutstandingBalance = 10;
            _accountVm.SelectedPlanSetupOption = PlanSetupOptions.DisposableIncome;

            _accountVm.DirectDebitEmailAddress = directDebitEmailAddress;
            _accountVm.DirectDebitIsEmailAddressFieldVisible = true;

            //Act
            var result = _validator.Validate(_accountVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(ValidationMessages.InvalidEmailAddress, errors[0].ErrorMessage);
        }

        [TestMethod]
        public void DirectDebit_SelectedPlanSetupOption_NotSet()
        {
            _accountVm = new PaymentOptionsVm()
            {
                AcceptTermsAndConditions = true,
                AverageMonthlyPayment = 48.00M,
                DirectDebitAmount = 48.00M,
                MonthlyDisposableIncome = 0.00M,
                DirectDebitEmailAddress = "test@test.com",
                DirectDebitSelectedFrequency = "monthly",
                DirectDebitSelectedStartDate = DateTime.Now.Date.ToString(),
                DiscountAccepted = false,
                DirectDebitStartDateEarliest = DateTime.Now.Date.AddDays(-1),
                DirectDebitStartDateLatest = DateTime.Now.AddDays(1),
                DiscountAmount = 0.00M,
                DiscountBalanceAvailable = false,
                DiscountPercentage = 0.00M,
                IandELessThanOrIs12MonthsOld = true,
                SelectedPaymentOption = "direct-debit",
                FullPaymentBalance = 123.45M,
                WithLowellSolicitors = false,
                OutstandingBalance = 123.45M
            };

            var result = _validator.Validate(_accountVm);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual("Please select an option", result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void DirectDebit_SelectedPlanSetupOption_DisposableIncome()
        {
            _accountVm = new PaymentOptionsVm()
            {
                AcceptTermsAndConditions = true,
                AverageMonthlyPayment = 48.00M,
                DirectDebitAmount = 48.00M,
                MonthlyDisposableIncome = 0.00M,
                DirectDebitEmailAddress = "test@test.com",
                DirectDebitSelectedFrequency = "monthly",
                DirectDebitSelectedStartDate = DateTime.Now.Date.ToString(),
                DiscountAccepted = false,
                DirectDebitStartDateEarliest = DateTime.Now.Date.AddDays(-1),
                DirectDebitStartDateLatest = DateTime.Now.AddDays(1),
                DiscountAmount = 0.00M,
                DiscountBalanceAvailable = false,
                DiscountPercentage = 0.00M,
                IandELessThanOrIs12MonthsOld = true,
                SelectedPaymentOption = "direct-debit",
                FullPaymentBalance = 123.45M,
                WithLowellSolicitors = false,
                OutstandingBalance = 123.45M,
                SelectedPlanSetupOption = PlanSetupOptions.DisposableIncome
            };

            var result = _validator.Validate(_accountVm);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void DirectDebit_SelectedPlanSetupOption_AverageSetupValue()
        {
            _accountVm = new PaymentOptionsVm()
            {
                AcceptTermsAndConditions = true,
                AverageMonthlyPayment = 48.00M,
                DirectDebitAmount = 48.00M,
                MonthlyDisposableIncome = 0.00M,
                DirectDebitEmailAddress = "test@test.com",
                DirectDebitSelectedFrequency = "monthly",
                DirectDebitSelectedStartDate = DateTime.Now.Date.ToString(),
                DiscountAccepted = false,
                DirectDebitStartDateEarliest = DateTime.Now.Date.AddDays(-1),
                DirectDebitStartDateLatest = DateTime.Now.AddDays(1),
                DiscountAmount = 0.00M,
                DiscountBalanceAvailable = false,
                DiscountPercentage = 0.00M,
                IandELessThanOrIs12MonthsOld = true,
                SelectedPaymentOption = "direct-debit",
                FullPaymentBalance = 123.45M,
                WithLowellSolicitors = false,
                OutstandingBalance = 123.45M,
                SelectedPlanSetupOption = PlanSetupOptions.AverageSetupValue
            };

            var result = _validator.Validate(_accountVm);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void DirectDebit_SelectedPlanSetupOption_OtherPaymentOffer()
        {
            _accountVm = new PaymentOptionsVm()
            {
                AcceptTermsAndConditions = true,
                AverageMonthlyPayment = 48.00M,
                DirectDebitAmount = 48.00M,
                MonthlyDisposableIncome = 0.00M,
                DirectDebitEmailAddress = "test@test.com",
                DirectDebitSelectedFrequency = "monthly",
                DirectDebitSelectedStartDate = DateTime.Now.Date.ToString(),
                DiscountAccepted = false,
                DirectDebitStartDateEarliest = DateTime.Now.Date.AddDays(-1),
                DirectDebitStartDateLatest = DateTime.Now.AddDays(1),
                DiscountAmount = 0.00M,
                DiscountBalanceAvailable = false,
                DiscountPercentage = 0.00M,
                IandELessThanOrIs12MonthsOld = true,
                SelectedPaymentOption = "direct-debit",
                FullPaymentBalance = 123.45M,
                WithLowellSolicitors = false,
                OutstandingBalance = 123.45M,
                SelectedPlanSetupOption = PlanSetupOptions.OtherPaymentOffer
            };

            var result = _validator.Validate(_accountVm);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void PartialPayment_PartialPaymentAmountGreaterThanFullPaymentBalance_ThenReturnErrorMessage()
        {
            //Arrange
            _accountVm.SelectedPaymentOption = PaymentOptionsSelectionsVm.Values.PartialPayment;
            _accountVm.DiscountAccepted = true;
            _accountVm.PartialPaymentAmount = 100;
            _accountVm.DiscountedBalance = 101;
            _accountVm.FullPaymentBalance = 99;
            _accountVm.PartialPaymentSelectedSourceOfFunds = "Disposable Income";

            //Act
            var result = _validator.Validate(_accountVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("You have entered a value greater than allowed. Please enter an amount less than or equal to £" + _accountVm.FullPaymentBalance, errors[0].ErrorMessage);
        }

        [TestMethod]
        public void PartialPayment_PartialPaymentAmountLessThanFullPaymentBalance_ThenReturnNoErrors()
        {
            //Arrange
            _accountVm.SelectedPaymentOption = PaymentOptionsSelectionsVm.Values.PartialPayment;
            _accountVm.DiscountAccepted = true;
            _accountVm.PartialPaymentAmount = 100;
            _accountVm.DiscountedBalance = 101;
            _accountVm.FullPaymentBalance = 101;
            _accountVm.PartialPaymentSelectedSourceOfFunds = "Disposable Income";

            //Act
            var result = _validator.Validate(_accountVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(0, errors.Count);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void PartialPayment_PartialPaymentAmountEqualToFullPaymentBalance_ThenReturnNoErrors()
        {
            //Arrange
            _accountVm.SelectedPaymentOption = PaymentOptionsSelectionsVm.Values.PartialPayment;
            _accountVm.DiscountAccepted = true;
            _accountVm.PartialPaymentAmount = 100;
            _accountVm.DiscountedBalance = 101;
            _accountVm.FullPaymentBalance = 100;
            _accountVm.PartialPaymentSelectedSourceOfFunds = "Disposable Income";

            //Act
            var result = _validator.Validate(_accountVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(0, errors.Count);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void ShowPayOffDiscountedDDPlanWarning_DDPlanInPlace_DiscountAccepted()
        {
            PaymentOptionsVm vm = new PaymentOptionsVm()
            {
                PlanInPlace = true,
                PlanIsDirectDebit = true,
                DiscountedBalancePreviouslyAccepted = true
            };

            Assert.IsTrue(vm.ShowPayOffDiscountedDDPlanWarning);
        }

        [TestMethod]
        public void ShowPayOffDiscountedDDPlanWarning_DDPlanInPlace_DiscountNotAccepted()
        {
            PaymentOptionsVm vm = new PaymentOptionsVm()
            {
                PlanInPlace = true,
                PlanIsDirectDebit = true,
                DiscountedBalancePreviouslyAccepted = false
            };

            Assert.IsFalse(vm.ShowPayOffDiscountedDDPlanWarning);
        }

        [TestMethod]
        public void ShowPayOffDiscountedDDPlanWarning_NonDDPlanInPlace_DiscountAccepted()
        {
            PaymentOptionsVm vm = new PaymentOptionsVm()
            {
                PlanInPlace = true,
                PlanIsDirectDebit = false,
                DiscountedBalancePreviouslyAccepted = true
            };

            Assert.IsTrue(vm.ShowPayOffDiscountedDDPlanWarning);
        }

        [TestMethod]
        public void ShowPayOffDiscountedDDPlanWarning_DDPlanNotInPlace_DiscountAccepted()
        {
            PaymentOptionsVm vm = new PaymentOptionsVm()
            {
                PlanInPlace = false,
                PlanIsDirectDebit = false,
                DiscountedBalancePreviouslyAccepted = true
            };

            Assert.IsFalse(vm.ShowPayOffDiscountedDDPlanWarning);
        }

        [TestMethod]
        public void ShowPayOffDiscountedDDPlanWarning_DDPlanNotInPlace_DiscountNotAccepted()
        {
            PaymentOptionsVm vm = new PaymentOptionsVm()
            {
                PlanInPlace = false,
                PlanIsDirectDebit = false,
                DiscountedBalancePreviouslyAccepted = false
            };

            Assert.IsFalse(vm.ShowPayOffDiscountedDDPlanWarning);
        }
    }
}
