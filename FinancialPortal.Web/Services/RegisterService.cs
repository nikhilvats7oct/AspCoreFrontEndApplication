using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinancialPortal.Web.Models;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Proxy.Interfaces;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Settings;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;

namespace FinancialPortal.Web.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly IApiGatewayProxy _apiGatewayProxy;
        private readonly IdentitySetting _identitySetting;
        private readonly ILogger<RegisterService> _logger;
        private readonly IRestClient _restClient;
        private readonly IPortalCryptoAlgorithm _portalCryptoAlgorithm;
        private readonly PortalSetting _portalSetting;

        public RegisterService(ILogger<RegisterService> logger,
            IApiGatewayProxy apiGatewayProxy, 
            IdentitySetting identitySetting,
            IRestClient restClient,
            IPortalCryptoAlgorithm portalCryptoAlgorithm,
            PortalSetting portalSetting)
        {
            _logger = logger;
            _identitySetting = identitySetting;
            _apiGatewayProxy = apiGatewayProxy;
            _restClient = restClient;
            _portalCryptoAlgorithm = portalCryptoAlgorithm;
            _portalSetting = portalSetting;
        }

        public Task<ResultDto> CheckDataProtection(DataProtectionDto dataProtectionDto)
        {
            var uri = "api/register/CheckDataProtection";

            var result = _restClient.PostAsync<DataProtectionDto, ResultDto>($"{_portalSetting.GatewayEndpoint}{uri}", dataProtectionDto);

            return result;
        }

        public Task<ResultDto> CheckIsWebRegistered(WebRegisteredDto webRegisteredDto)
        {
            var uri = "api/register/CheckIsWebRegistered";

            var result = _restClient.PostAsync<WebRegisteredDto, ResultDto>($"{_portalSetting.GatewayEndpoint}{uri}", webRegisteredDto);

            return result;
        }

        public async Task CompleteRegistration(CompleteRegistrationDto completeRegistrationDto)
        {
            await _apiGatewayProxy.CompleteRegistration(completeRegistrationDto);
        }

        public async Task<RegisterAccountResult> CreateAccount(RegisterAccount registerAccount)
        {
            var caseflowUserId = Guid.NewGuid().ToString();

            var item = new IdentityServerRegisterAccount
            {
                EmailAddress = registerAccount.EmailAddress,
                Password = registerAccount.Password,
                Username = registerAccount.EmailAddress,
                ClientId = _identitySetting.ClientId,
                ClientSecret = _identitySetting.ClientSecret,
                Properties = new Dictionary<string, string>
                {
                    {Constants.CaseflowUserId, caseflowUserId},
                    {
                        Constants.CaseflowReference,
                        _portalCryptoAlgorithm.EncryptUsingAes(registerAccount.LowellReferenceNumber)
                    }
                }
            };

            var userCreationResult =
                await _restClient.PostAsync<IdentityServerRegisterAccount, RegisterAccountResult>(
                    $"{_identitySetting.Authority}{_identitySetting.RegisterEndpoint}", item);

            if (userCreationResult.IsSuccess)
            {
                return new RegisterAccountResult
                {
                    IsSuccess = true,
                    SubjectId = userCreationResult.SubjectId
                };
            }

            return new RegisterAccountResult
            {
                IsSuccess = false,
                Message = userCreationResult.Message,
                ErrorCode = userCreationResult.ErrorCode
            };
        }

        public async Task<ResendActivationLinkResult> ResendActivationLink(string emailAddress)
        {
            var item = new IdentityServerResendEmailConfirmation
            {
                Username = emailAddress,
                ClientId = _identitySetting.ClientId,
                ClientSecret = _identitySetting.ClientSecret
            };

            var resendResult =
                await _restClient.PostAsync<IdentityServerResendEmailConfirmation, ResendActivationLinkResult>(
                    $"{_identitySetting.Authority}{_identitySetting.ResendEmailConfirmationEmailEndpoint}", item);

            if (resendResult.IsSuccess)
            {
                return new ResendActivationLinkResult
                {
                    IsSuccess = true
                };
            }

            return new ResendActivationLinkResult
            {
                IsSuccess = false,
                Message = resendResult.Message
            };
        }

        public async Task<bool> IsPendingRegistration(string reference)
        {

            var request = new IdentityServerFindAccount
            {
                ClientId = _identitySetting.ClientId,
                ClientSecret = _identitySetting.ClientSecret,
                Properties = new Dictionary<string, string>
                {
                    { Constants.CaseflowReference, _portalCryptoAlgorithm.EncryptUsingAes(reference)}
                }
            };

            var result =
                await _restClient.PostAsync<IdentityServerFindAccount, string[]>(
                    $"{_identitySetting.Authority}{_identitySetting.FindAccountEndpoint}", request);

            return result != null && result.Any();
        }

        public async Task UpdateIdentityServerAccountWithAccountConfirmationProperty(string subjectId)
        {
            var updateProperty = new IdentityServerUpdateProperty
            {
                SubjectId = Guid.Parse(subjectId),
                ClientId = _identitySetting.ClientId,
                ClientSecret = _identitySetting.ClientSecret,
                PropertiesToAddOrReplace = new Dictionary<string, string>
                {
                    // Set account to confirmed status
                    {Constants.CaseflowUserConfirmed, "true"}
                },
                PropertiesToRemove = new List<string>
                {
                    // Remove caseflow ref as it's no longer needed.
                    Constants.CaseflowReference
                }
            };

            var updateAccountPropertyResponse =
                await _restClient.PostAsync<IdentityServerUpdateProperty, LinkAccountResult>(
                    $"{_identitySetting.Authority}{_identitySetting.UpdatePropertiesEndpoint}", updateProperty);

            if (!updateAccountPropertyResponse.IsSuccess)
            {
                throw new Exception(updateAccountPropertyResponse.Message);
            }
        }

        private class IdentityServerUpdateProperty
        {
            public string Username { get; set; }
            public Guid? SubjectId { get; set; }
            public string ClientId { get; set; }
            public string ClientSecret { get; set; }

            public Dictionary<string, string> PropertiesToAddOrReplace { get; set; }
            public List<string> PropertiesToRemove { get; set; }
        }

        private class IdentityServerRegisterAccount
        {
            public IdentityServerRegisterAccount()
            {
                Properties = new Dictionary<string, string>();

                // TODO: Remove this!
                DoNotEnforcePasswordPolicy = true;
            }

            public string EmailAddress { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string ClientId { get; set; }
            public string ClientSecret { get; set; }
            public DateTime? ExpiresAt { get; set; }
            public Dictionary<string, string> Properties { get; set; }
            public bool DoNotEnforcePasswordPolicy { get; }
        }

        private class IdentityServerResendEmailConfirmation
        {
            public string Username { get; set; }

            public string ClientId { get; set; }

            public string ClientSecret { get; set; }
        }

        private class IdentityServerFindAccount
        {
            public IDictionary<string, string> Properties { get; set; }

            public string ClientId { get; set; }

            public string ClientSecret { get; set; }
        }
    }
}