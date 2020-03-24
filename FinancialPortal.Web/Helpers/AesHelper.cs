using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace FinancialPortal.Web.Helpers
{
    public static class AesHelper
    {
        public static string Encrypt(string dataToEncrypt, string key, string iv)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key); 
            var ivBytes = Encoding.UTF8.GetBytes(iv); 

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

        public static string Decrypt(string dataToDecrypt, string key, string iv)
        {
            var rawDataToDecrypt = Convert.FromBase64String(dataToDecrypt);
            var keyBytes = Encoding.UTF8.GetBytes(key); 
            var ivBytes = Encoding.UTF8.GetBytes(iv);

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
    }
}