using FinancialPortal.Web.Validation;
using FinancialPortal.Web.ViewModels;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FinancialPortal.UnitTests.ViewModels
{
    [TestClass]
    public class LoginVmValidatorTests
    {
        private LoginVmValidator _validator;

        [TestInitialize]
        public void Initialise()
        {
            _validator = new LoginVmValidator();
        }

        [TestMethod]
        public void LoginVmValidator_ValidModel()
        {
            LoginVm model = new LoginVm()
            {
                EmailAddress = "a@b.com",
                Password = "P@ssw0rd.1"
            };

            ValidationResult result = _validator.Validate(model);
            Assert.IsTrue(result.IsValid);
        }

        [DataRow(null)]
        [DataRow("")]
        [TestMethod]
        public void LoginVmValidator_MissingEmail(string email)
        {
            LoginVm model = new LoginVm()
            {
                EmailAddress = email,
                Password = "P@ssw0rd.1"
            };

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(ValidationMessages.MissingEmailAddress, result.Errors[0].ErrorMessage);
        }

        [DataRow(null)]
        [DataRow("")]
        [TestMethod]
        public void LoginVmValidator_MissingPassword(string password)
        {
            LoginVm model = new LoginVm()
            {
                EmailAddress = "a@b.com",
                Password = password
            };

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(ValidationMessages.MissingPassword, result.Errors[0].ErrorMessage);
        }

        [DataRow(null, null)]
        [DataRow("", "")]
        [DataRow("", null)]
        [DataRow(null, "")]
        [TestMethod]
        public void LoginVmValidator_MissingPasswordAndEmail(string email, string password)
        {
            LoginVm model = new LoginVm()
            {
                EmailAddress = email,
                Password = password
            };

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(2, result.Errors.Count);
            Assert.AreEqual(ValidationMessages.MissingEmailAddress, result.Errors[0].ErrorMessage);
            Assert.AreEqual(ValidationMessages.MissingPassword, result.Errors[1].ErrorMessage);
        }

    }
}
