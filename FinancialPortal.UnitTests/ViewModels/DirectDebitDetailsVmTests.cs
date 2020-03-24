using FinancialPortal.Web.Validation;
using FinancialPortal.Web.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FinancialPortal.UnitTests.ViewModels
{
    [TestClass]
    public class DirectDebitDetailsVmTests
    {

        DirectDebitDetailsVmValidator _validator;
        private DirectDebitDetailsVm _directDebitDetailsVm;

        [TestInitialize]
        public void Initialise()
        {
            _validator = new DirectDebitDetailsVmValidator();
            _directDebitDetailsVm = new DirectDebitDetailsVm()
            {
                AcceptDirectDebitGuarantee = true,
                AccountHoldersName = "Mr Test",
                AccountNumber = "12345678",
                SortCode = "123456"
            };
        }

        [DataRow("!")]
        [DataRow("\"")]
        [DataRow("£")]
        [DataRow("$")]
        [DataRow("%")]
        [DataRow("^")]
        [DataRow("&")]
        [DataRow("*")]
        [DataRow("(")]
        [DataRow(")")]
        [DataRow("_")]
        [DataRow("+")]
        [DataRow("[")]
        [DataRow("]")]
        [DataRow("{")]
        [DataRow("}")]
        [DataRow(":")]
        [DataRow("@")]
        [DataRow("#")]
        [DataRow("~")]
        [DataRow("#")]
        [DataRow("<")]
        [DataRow(",")]
        [DataRow(">")]
        [DataRow(".")]
        [DataRow("/")]
        [DataRow("?")]
        [TestMethod]
        public void DirectDebitDetails_WhenIncorrectCharactersInName_ThenReturnErrorMessage(string accountHoldersName)
        {
            //Arrange
            _directDebitDetailsVm.AccountHoldersName = accountHoldersName;

            //Act
            var result = _validator.Validate(_directDebitDetailsVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Account name can only contains alphanumeric characters, spaces, hyphens and apostrophes only", errors[0].ErrorMessage);
        }

        [DataRow("ValidName")]
        [DataRow("Valid Name")]
        [DataRow("Valid Name-")]
        [DataRow("Valid Name-'")]
        [TestMethod]
        public void DirectDebitDetails_WhenCorrectCharactersInName_ThenPassValidation(string accountHoldersName)
        {
            //Arrange
            _directDebitDetailsVm.AccountHoldersName = accountHoldersName;

            //Act
            var result = _validator.Validate(_directDebitDetailsVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(0, errors.Count);

        }

        [TestMethod]
        public void DirectDebitDetails_WhenAccountHoldersNameBlank_ThenReturnErrorMessage()
        {
            //Arrange
            _directDebitDetailsVm.AccountHoldersName = "";

            //Act
            var result = _validator.Validate(_directDebitDetailsVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Please enter the account holders name", errors[0].ErrorMessage);
        }

        [TestMethod]
        public void DirectDebitDetails_WhenAccountHoldersNameValid_ThenPassValidation()
        {
            //Arrange
            _directDebitDetailsVm.AccountHoldersName = "Blah blah";

            //Act
            var result = _validator.Validate(_directDebitDetailsVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(0, errors.Count);
        }

        [TestMethod]
        public void DirectDebitDetails_WhenAccountHoldersNameMoreThan40CharactersLong_ThenReturnErrorMessage()
        {
            //Arrange
            _directDebitDetailsVm.AccountHoldersName = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"; //41 chars long

            //Act
            var result = _validator.Validate(_directDebitDetailsVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Name must be less than 40 characters", errors[0].ErrorMessage);
        }


        [DataRow("1234567")]
        [DataRow("123456789")]
        [TestMethod]
        public void DirectDebitDetails_AccountNumberIncorrect_ThenReturnErrorMessage(string accountNumber)
        {
            //Arrange
            _directDebitDetailsVm.AccountNumber = accountNumber;

            //Act
            var result = _validator.Validate(_directDebitDetailsVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Account Number must be 8 digits long", errors[0].ErrorMessage);
        }

        [DataRow("12345678")]
        [TestMethod]
        public void DirectDebitDetails_AccountNumberCorrect_ThenPassValidation(string accountNumber)
        {
            //Arrange
            _directDebitDetailsVm.AccountNumber = accountNumber;

            //Act
            var result = _validator.Validate(_directDebitDetailsVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(0, errors.Count);
        }

        [DataRow("12345")]
        [DataRow("1234567")]
        [TestMethod]
        public void DirectDebitDetails_SortCodeIncorrect_ThenReturnErrorMessage(string sortCode)
        {
            //Arrange
            _directDebitDetailsVm.SortCode = sortCode;

            //Act
            var result = _validator.Validate(_directDebitDetailsVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Sort code must be 6 digits long", errors[0].ErrorMessage);
        }


        [DataRow("123456")]
        [TestMethod]
        public void DirectDebitDetails_SortCodeIncorrect_ThenPassValdation(string sortCode)
        {
            //Arrange
            _directDebitDetailsVm.SortCode = sortCode;

            //Act
            var result = _validator.Validate(_directDebitDetailsVm);

            //Assert
            var errors = result.Errors;
            Assert.AreEqual(0, errors.Count);
        }
    }
}
