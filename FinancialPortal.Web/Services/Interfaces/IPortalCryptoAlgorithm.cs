using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Services.Interfaces
{
    public interface IPortalCryptoAlgorithm
    {
        string EncryptUsingAes(string dataToEncrypt);

        string DecryptUsingAes(string dataToDecrypt);

        string EncryptUsingDataProtector(string dataToEncrypt);

        string DecryptUsingDataProtector(string dataToDecrypt);
    }
}
