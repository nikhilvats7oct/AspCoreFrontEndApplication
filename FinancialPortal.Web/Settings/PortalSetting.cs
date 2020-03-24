namespace FinancialPortal.Web.Settings
{
    public class PortalSetting
    {
        public string TalkToUsUrl { get; set; }

        public int ViewTransactionsPageSize { get; set; } 

        public string ShaSalt { get; set; }

        public string RedisConfiguration { get; set; }

        public string GatewayEndpoint { get; set; }

        public string SolicitorsRedirectDataProtectionUrl { get; set; }

        public string SolicitorsRedirectUrl { get; set; }

        public int PageSizeViewTransactions { get; set; }

        public string GTMContainer { get; set; }

        public decimal MonthlyDisposableIncomePlanSetupPercentage { get; set; }

        public decimal AverageMonthlyPaymentAmount { get; set; }

        public bool AllowLowellEmailAddresses { get; set; }

        public string PlanTransferOptOutNumber { get; set; }

        public Features Features { get; set; }

        public int MinimumWorkerThreads { get; set; }
        public int MinimumIocpThreads { get; set; }
    }
}
