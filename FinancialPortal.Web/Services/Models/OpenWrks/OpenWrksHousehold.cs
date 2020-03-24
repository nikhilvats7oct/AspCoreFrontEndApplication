using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialPortal.Web.Services.Models.OpenWrks
{
    public class OpenWrksHousehold
    {
        public bool LivesWithPartner { get; set; }

        public int DependentChildren { get; set; }

        public int DependentYoungAdults { get; set; }

        public int DependentAdults { get; set; }

        public int NonDependentCohabitants { get; set; }
    }
}
