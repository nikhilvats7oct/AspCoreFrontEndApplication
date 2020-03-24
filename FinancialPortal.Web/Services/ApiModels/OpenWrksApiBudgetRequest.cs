﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Services.ApiModels
{
    public class OpenWrksApiBudgetRequest
    {
        public string CustomerReference { get; set; }

        public string JourneyEndRedirectUrl { get; set; }
    }
}
