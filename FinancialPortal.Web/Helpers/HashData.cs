using System;
using System.Security.Cryptography;
using System.Text;

namespace FinancialPortal.Web.Helpers
{
    public class HashData
    {
        public static string Sha256(string plainText, string salt)
        {
            byte[] key = Encoding.UTF8.GetBytes(salt ?? "salt");
            string toBeHashed = string.Concat(plainText, key);

            using (SHA256 sha256 = SHA256.Create())
            {
                return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(toBeHashed)));
            }
        }
    }
}