using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using FinancialPortal.Web.Helpers;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration.Json;
using Newtonsoft.Json.Linq;
using Serilog;

namespace FinancialPortal.Web.Configuration
{
    public class SecureJsonConfigurationProvider : JsonConfigurationProvider
    {
        private const string EncryptedPayloadPropertyName = "encryptedPayload";
        private const string EncryptedPayloadKeyPropertyName = "encryptedPayloadKey";

        public SecureJsonConfigurationProvider(JsonConfigurationSource source) : base(source)
        {
        }

        public override void Load(Stream stream)
        {
            try
            {
                stream.Position = 0;
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    var encryptedJson = reader.ReadToEnd();
                    var decryptedJson = DecryptJson(encryptedJson) ?? "";
                    var cleanedJson = CleanJson(decryptedJson);

                    var decryptedStream = GenerateStreamFromString(cleanedJson);

                    base.Load(decryptedStream);
                }
            }
            catch (Exception e)
            {
                var logger = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .WriteTo.RollingFile("appsettingsdecrypt.log")
                    .CreateLogger();

                logger.Error(e,
                    $"An unknown error has occurred while trying to do appsettings decryption. {e}.");

                throw;
            }
        }

        private static string CleanJson(string jsonToClean)
        {
            var firstIndexOfBracket = jsonToClean.IndexOf("{", StringComparison.CurrentCultureIgnoreCase);

            return jsonToClean.Substring(
                firstIndexOfBracket, jsonToClean.Length - firstIndexOfBracket);
        }

        private static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        private string DecryptJson(string encryptedJson)
        {
            var jsonObject = JObject.Parse(encryptedJson);

            if (!jsonObject.ContainsKey(EncryptedPayloadPropertyName) ||
                !jsonObject.ContainsKey(EncryptedPayloadKeyPropertyName))
            {
                return encryptedJson;
            }

            var encryptedPayloadKey = jsonObject[EncryptedPayloadKeyPropertyName].ToString();
            var encryptedPayload = jsonObject[EncryptedPayloadPropertyName].ToString();

            string decryptedKey;

            try
            {
                decryptedKey = DecryptDataOaepSha1(LoadCertificate(), encryptedPayloadKey);
            }
            catch (Exception e)
            {
                var logger = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .WriteTo.RollingFile("appsettingsdecrypt.log")
                    .CreateLogger();

                logger.Error(e,
                    $"An error occurred while fetching AES key from cert. Switching from config. {e}.");

                decryptedKey = System.Configuration.ConfigurationManager.AppSettings["EncryptionKey"] ?? "UNKNOWN";
            }

            var split = decryptedKey.Split(';');

            var aesKey = split[0];
            var aesIv = split[1];

            return AesHelper.Decrypt(encryptedPayload, aesKey, aesIv);
        }

        private X509Certificate2 LoadCertificate()
        {
            var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            var certificate = store.Certificates.Find(X509FindType.FindBySubjectDistinguishedName,
                "CN=LslPortalConfigurationEncryption", false);

            if (!certificate.Any())
            {
                throw new SecurityException(
                    "Unable to find a certificate with SubjectName LslPortalConfigurationEncryption in CurrentUser's Personal Store. Check the AppPool for CurrentUser information.");
            }

            return certificate[0];
        }

        private static string EncryptDataOaepSha1(X509Certificate2 certificate, string data)
        {
            // GetRSAPublicKey returns an object with an independent lifetime, so it should be
            // handled via a using statement.
            using (var rsa = certificate.GetRSAPublicKey())
            {
                // OAEP allows for multiple hashing algorithms, what was formermly just "OAEP" is
                // now OAEP-SHA1.
                var encryptedBytes = rsa.Encrypt(Encoding.UTF8.GetBytes(data), RSAEncryptionPadding.OaepSHA1);

                return Convert.ToBase64String(encryptedBytes);
            }
        }

        private static string DecryptDataOaepSha1(X509Certificate2 certificate, string data)
        {
            // GetRSAPrivateKey returns an object with an independent lifetime, so it should be
            // handled via a using statement.
            using (var rsa = certificate.GetRSAPrivateKey())
            {
                var decryptedBytes = rsa.Decrypt(Convert.FromBase64String(data), RSAEncryptionPadding.OaepSHA1);

                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
    }
}