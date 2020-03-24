using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialPortal.Web
{
    public class EncryptionSetting
    {
        public string  AesKey { get; set; }
        public string AesInitializationVector { get; set; }
    }
}
