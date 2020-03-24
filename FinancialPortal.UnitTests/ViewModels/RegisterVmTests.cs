using FinancialPortal.Web.Settings;
using FinancialPortal.Web.Validation;
using FinancialPortal.Web.ViewModels;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FinancialPortal.UnitTests.ViewModels
{
    [TestClass]
    public class RegisterVmTests
    {
        private RegisterVmValidator _validator;


        [TestInitialize]
        public void SetUp()
        {
            _validator = new RegisterVmValidator(new PortalSetting { AllowLowellEmailAddresses = false });
        }

        [TestMethod]
        [DataRow("test@test.com")]
        [DataRow("abc@abc.com.abc")]
        public void RegisterVmValidator_EmailAddress_MatchesCorrectEmail_ReturnsNoErrors(string email)
        {
            var model = new RegisterVm()
            {
                EmailAddress = email,
                Password = "testabcd1A$",
                ConfirmPassword = "testabcd1A$",
                TsAndCsAccepted = true
            };

            ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(true, result.IsValid);
        }

        [TestMethod]
        [DataRow("test@test", 1)]
        [DataRow("abc@", 1)]
        [DataRow("test@lowell.com", 1)]
        [DataRow("", 2)]
        public void RegisterVmValidator_EmailAddress_MatchesIncorrectEmailAddress_ReturnsErrors(string email, int errorCount)
        {
            var model = new RegisterVm()
            {
                EmailAddress = email,
                Password = "testabcd1A$",
                ConfirmPassword = "testabcd1A$",
                TsAndCsAccepted = true,
            };

            ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(false, result.IsValid);
        }

        [TestMethod]
        [DataRow(null, 1)]
        [DataRow("", 5)]
        [DataRow("test", 4)]
        [DataRow("testabcd", 3)]
        [DataRow("testabcd1", 2)]
        [DataRow("testabcd1A", 1)]
        [DataRow("tE$7", 1)]
        public void RegisterVmValidator_IncorrectPassword_ReturnsErrors(string password, int errorCount)
        {
            var model = new RegisterVm()
            {
                EmailAddress = "Test@test.com",
                Password = password,
                ConfirmPassword = password,
                TsAndCsAccepted = true
            };

            ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(false, result.IsValid);
        }

        [TestMethod]
        [DataRow("testabcd1A$")]
        [DataRow("testabcd1A.")]
        public void RegisterVmValidator_CorrectPassword(string password)
        {
            var model = new RegisterVm()
            {
                EmailAddress = "Test@test.com",
                Password = password,
                ConfirmPassword = password,
                TsAndCsAccepted = true
            };

            ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(true, result.IsValid);
        }

        [TestMethod]
        [DataRow("testabcd1A$", null, 5)]
        [DataRow("testabcd1A$", "", 5)]
        public void RegisterVmValidator_IncorrectConfirmPassword_ReturnsErrors(string password, string confirmPassword, int errorCount)
        {
            var model = new RegisterVm()
            {
                EmailAddress = "Test@test.com",
                Password = password,
                ConfirmPassword = confirmPassword,
                TsAndCsAccepted = true
            };

            ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(false, result.IsValid);
        }

        [TestMethod]
        [DataRow("testabcd1A$", "testabcd1A$")]
        [DataRow("testabcd1A.", "testabcd1A.")]
        public void RegisterVmValidator_CorrectConfirmPassword(string password, string confirmPassword)
        {
            var model = new RegisterVm()
            {
                EmailAddress = "Test@test.com",
                Password = password,
                ConfirmPassword = confirmPassword,
                TsAndCsAccepted = true
            };

            ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(true, result.IsValid);
        }


        [TestMethod]
        [DataRow("a")]
        public void RegisterVmValidator_HoneyPotTextBoxNotEmpty_ReturnsErrors(string honeyPot)
        {
            var model = new RegisterVm()
            {
                EmailAddress = "Test@test.com",
                Password = "testabcd1A$",
                ConfirmPassword = "testabcd1A$",
                HoneyPotTextBox = honeyPot,
                TsAndCsAccepted = true
            };

            var result = _validator.Validate(model);

            Assert.AreEqual(false, result.IsValid);
        }

        [TestMethod]
        [DataRow("")]
        public void RegisterVmValidator_HoneyPotTextBoxEmpty_ReturnsPass(string honeyPot)
        {
            var model = new RegisterVm()
            {
                EmailAddress = "Test@test.com",
                Password = "testabcd1A$",
                ConfirmPassword = "testabcd1A$",
                HoneyPotTextBox = honeyPot,
                TsAndCsAccepted = true
            };

            var result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
        }

        [DataRow("test@test.com")]
        [DataRow("lowell@test.com")] //can contain lowell before the @
        [DataRow("test.test@test.com")] //can contain period
        [DataRow("test-test@test.com")] //can contain hyphen
        [DataRow("test_test@test.com")] //can contain underscore
        [TestMethod]
        public void RegisterVmValidator_ValidEmail(string email)
        {
            var model = new RegisterVm()
            {
                EmailAddress = email,
                Password = "testabcd1A$",
                ConfirmPassword = "testabcd1A$",
                TsAndCsAccepted = true
            };

            var result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(0, result.Errors.Count);
        }

        [DataRow("test#test@test.com")]
        [DataRow("test$test@test.com")]
        [DataRow("test%test@test.com")]
        [DataRow("test!test@test.com")]
        [DataRow("test&test@test.com")]
        [DataRow("test£test@test.com")]
        [DataRow("test*test@test.com")]
        [DataRow("test.test@lowell.com")]
        [DataRow("test.test@lowellgroup.com")]
        [DataRow(null)]
        [DataRow("")]
        [TestMethod]
        public void RegisterVmValidator_InvalidEmail(string email)
        {
            var model = new RegisterVm()
            {
                EmailAddress = email,
                Password = "testabcd1A$",
                ConfirmPassword = "testabcd1A$",
                TsAndCsAccepted = true
            };

            var result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(ValidationMessages.InvalidEmailAddress, result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void RegisterVmValidator_TsAndCsNotAccepted()
        {
            var model = new RegisterVm()
            {
                EmailAddress = "stewart.hartley@testing.co.uk",
                Password = "testabcd1A$",
                ConfirmPassword = "testabcd1A$",
                TsAndCsAccepted = false
            };

            var result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(ValidationMessages.NotAcceptedTermsAndConditions, result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        [DataRow("stewart.hartley@lowellgroup.co.uk")]
        [DataRow("stewart.hartley@lowell.co.uk")]
        public void EmailAddress_LowellEmailsAllowed(string emailAddress)
        {
            RegisterVm vm = new RegisterVm()
            {
                EmailAddress = emailAddress,
                Password = "testabcd1A$",
                ConfirmPassword = "testabcd1A$",
                TsAndCsAccepted = true
            };

            _validator = new RegisterVmValidator(new PortalSetting { AllowLowellEmailAddresses = true });
            ValidationResult result = _validator.Validate(vm);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        [DataRow("Password123!")]
        [DataRow("PASSword123\"")]
        [DataRow("Password£123")]
        [DataRow("Passw$ord123")]
        [DataRow("%Password123")]
        [DataRow("Passwo^rd123")]
        [DataRow("Pass&word123")]
        [DataRow("Password12*3")]
        [DataRow("Password(123")]
        [DataRow("P)assword123")]
        [DataRow("Pass-word123")]
        [DataRow("Pass_Word123")]
        [DataRow("Password+123")]
        [DataRow("123Passwo=rd")]
        [DataRow("Pa|ssword123")]
        [DataRow(@"Password123\")]
        [DataRow("PassWORD123/")]
        [DataRow("Password??123")]
        [DataRow("123Password...")]
        [DataRow("Password,123")]
        [DataRow("PassWORD<123")]
        [DataRow("PASSword12>3")]
        [DataRow("Password::123")]
        [DataRow("Passw;ord123")]
        [DataRow("Passw@ord123")]
        [DataRow("Pass'word123")]
        [DataRow("PassWOrd1~23")]
        [DataRow("P#ass123word")]
        [DataRow("Passw{ord123")]
        [DataRow("pASSw}ord123")]
        [DataRow("Password12[3")]
        [DataRow("Pas]sword123")]
        public void PasswordValid(string password)
        {
            RegisterVm vm = new RegisterVm()
            {
                Password = password,
                ConfirmPassword = password,
                EmailAddress = "test@test.com",
                TsAndCsAccepted = true
            };

            ValidationResult result = _validator.Validate(vm);

            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(0, result.Errors.Count);
        }

        [TestMethod]
        [DataRow("Pwd123!")] //Too short
        [DataRow("Passwordjhghkjhljkhjkhlkljhlkjhljkhljkhlkjhlkjhkjhkljhkjhlkjkgjkgjkgjgkgkgj123!")] //Too long
        [DataRow("password123!")] //No uppercase char
        [DataRow("PASSWORD123!")] //No lowercase char
        [DataRow("PasswordABC!")] //No digit
        [DataRow("Password123")] //No special char
        public void PasswordInvalid(string password)
        {
            RegisterVm vm = new RegisterVm()
            {
                EmailAddress = "test@test.com",
                Password = password,
                ConfirmPassword = password,
                TsAndCsAccepted = true
            };

            ValidationResult result = _validator.Validate(vm);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(2, result.Errors.Count);
            Assert.AreEqual(ValidationMessages.BadFormatPassword, result.Errors[0].ErrorMessage);
            Assert.AreEqual(ValidationMessages.BadFormatPassword, result.Errors[1].ErrorMessage);
        }

        [TestMethod]
        public void PasswordMissMatch()
        {
            RegisterVm vm = new RegisterVm()
            {
                EmailAddress = "test@test.com",
                Password = "Password123!",
                ConfirmPassword = "123Password!",
                TsAndCsAccepted = true
            };

            ValidationResult result = _validator.Validate(vm);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(2, result.Errors.Count);
            Assert.AreEqual(ValidationMessages.MismatchPassword, result.Errors[0].ErrorMessage);
            Assert.AreEqual(ValidationMessages.MismatchPassword, result.Errors[1].ErrorMessage);
        }

    }
}
