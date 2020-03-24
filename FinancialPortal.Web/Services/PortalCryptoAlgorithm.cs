using FinancialPortal.Web.Services.Interfaces;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.DataProtection;

namespace FinancialPortal.Web.Services
{
    public class PortalCryptoAlgorithm : IPortalCryptoAlgorithm
    {
        private readonly IDiscoveryCache _discoveryCache;
        private readonly EncryptionSetting _encryptionSetting;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDataProtector _dataProtector;

        public PortalCryptoAlgorithm(
            IHttpContextAccessor httpContextAccessor,
            IDiscoveryCache discoveryCache,
            IDataProtectionProvider dataProtectionProvider,
            EncryptionSetting encryptionSetting)
        {
            _httpContextAccessor = httpContextAccessor;
            _discoveryCache = discoveryCache;
            _encryptionSetting = encryptionSetting;

            _dataProtector = dataProtectionProvider.CreateProtector(nameof(PortalCryptoAlgorithm));
        }

        public string EncryptUsingAes(string dataToEncrypt)
        {
            var keyBytes = Encoding.UTF8.GetBytes(_encryptionSetting.AesKey);
            var ivBytes = Encoding.UTF8.GetBytes(_encryptionSetting.AesInitializationVector);

            using (var aes = new AesCryptoServiceProvider())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                aes.Key = keyBytes;
                aes.IV = ivBytes;

                using (var memoryStream = new MemoryStream())
                {
                    var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(),
                        CryptoStreamMode.Write);

                    var bytes = Encoding.UTF8.GetBytes(dataToEncrypt);
                    cryptoStream.Write(bytes, 0, bytes.Length);
                    cryptoStream.FlushFinalBlock();

                    return Convert.ToBase64String(memoryStream.ToArray());
                }
            }
        }

        public string DecryptUsingAes(string dataToDecrypt)
        {
            var rawDataToDecrypt = Convert.FromBase64String(dataToDecrypt);
            var keyBytes = Encoding.UTF8.GetBytes(_encryptionSetting.AesKey);
            var ivBytes = Encoding.UTF8.GetBytes(_encryptionSetting.AesInitializationVector);

            using (var aes = new AesCryptoServiceProvider())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                aes.Key = keyBytes;
                aes.IV = ivBytes;

                using (var memoryStream = new MemoryStream())
                {
                    var cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(),
                        CryptoStreamMode.Write);

                    cryptoStream.Write(rawDataToDecrypt, 0, rawDataToDecrypt.Length);
                    cryptoStream.FlushFinalBlock();

                    var decryptBytes = memoryStream.ToArray();

                    return Encoding.UTF8.GetString(decryptBytes);
                }
            }
        }

        public string EncryptUsingDataProtector(string dataToEncrypt)
        {
            return _dataProtector.Protect(dataToEncrypt);
        }

        public string DecryptUsingDataProtector(string dataToDecrypt)
        {
            return _dataProtector.Unprotect(dataToDecrypt);
        }

    }
}
