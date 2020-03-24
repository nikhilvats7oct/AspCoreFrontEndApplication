using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace FinancialPortal.Web.Security
{
    public class CertificateSecurityHelper
    {
        public static string EncryptDataOaepSha1(string data)
        {
            // GetRSAPublicKey returns an object with an independent lifetime, so it should be
            // handled via a using statement.
            using (var rsa = GetCertificate().GetRSAPublicKey())
            {
                // OAEP allows for multiple hashing algorithms, what was formermly just "OAEP" is
                // now OAEP-SHA1.
                var encryptedBytes = rsa.Encrypt(Encoding.UTF8.GetBytes(data), RSAEncryptionPadding.OaepSHA1);

                return Convert.ToBase64String(encryptedBytes);
            }
        }

        public static string DecryptDataOaepSha1(string data)
        {
            // GetRSAPrivateKey returns an object with an independent lifetime, so it should be
            // handled via a using statement.
            using (var rsa = GetCertificate().GetRSAPrivateKey())
            {
                var decryptedBytes = rsa.Decrypt(Convert.FromBase64String(data), RSAEncryptionPadding.OaepSHA1);

                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }

        public static X509Certificate2 GetCertificate()
        {
            var temp = System.AppContext.BaseDirectory;
            //temp = AppDomain.CurrentDomain.BaseDirectory;
            //temp = Directory.GetCurrentDirectory();
            //temp = Environment.CurrentDirectory;
            
            const string certificateFileName = "signingcert.pfx";
            var certBytes = File.ReadAllBytes($"{temp}\\{certificateFileName}");
            return new X509Certificate2(certBytes, "P@ssw0rd");
        }
    }
}