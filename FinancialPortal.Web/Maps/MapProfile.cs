using AutoMapper;
using FinancialPortal.Models.ViewModels;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Services.ApiModels;
using FinancialPortal.Web.Services.Models;
using FinancialPortal.Web.Services.Models.OpenWrks;
using FinancialPortal.Web.ViewModels;
using System.Collections.Generic;
using X.PagedList;

namespace FinancialPortal.Web.Maps
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<OneOffPaymentReviewVm, VerifoneTransactionDto>();
            CreateMap<OneOffPaymentReviewVm, OneOffPaymentDto>();

            CreateMap<DataProtectionVm, DataProtectionDto>();
            CreateMap<DataProtectionVm, WebRegisteredDto>();
            CreateMap<RegisterVm, WebRegisteredDto>();
            CreateMap<RegisterVm, RegistrationEmailDto>();
            CreateMap<RegisterVm, CompleteRegistrationDto>()
                .ForMember("IsSuccessful", i => i.Ignore())
                .ForMember("MessageForUser", i => i.Ignore());

            CreateMap<ApiAccountSummary, AccountSummary>();
            CreateMap<ApiAccountSummaries, AccountSummaries>();
            CreateMap<ApiAccount, Account>();
            CreateMap<ApiTransaction, Transaction>();
            CreateMap<CustomerSummary, MyAccountsVm>().ConvertUsing<CustomerSummaryToMyAccountsVmConverter>();
            CreateMap<Transaction, TransactionVm>().ConvertUsing<TransactionToTransactionVmConverter>();
            CreateMap<Account, MyAccountsDetailVm>().ConvertUsing<AccountToMyAccountsDetailVmConverter>();
            CreateMap<OpenWrksBudget, IncomeAndExpenditure>().ConvertUsing<OpenWrksAndIandEMappingConverter>();
            CreateMap<OrderByVm, FilterDocumentsVm>().ConvertUsing<OrderByVmToFilterDocumentsVmConverter>();
            CreateMap<DocumentAccountsVm, LinkedAccountsVm>().ConvertUsing<DocumentAccountsVmToOtherAccountsVmConverter>();
            CreateMap<FilterDocumentsVm, FilterDocumentsVm>().ConvertUsing<FilterDocumentsVmToFilterDocumentsVmConverter>();

            CreateMap<DocumentAccountsVm, MyDocumentsVm>().ConvertUsing<ListDocumentAccountsVmToMyDocumentsVmConverter>();
            CreateMap<IPagedList<DocumentVm>, MyDocumentsVm>().ConvertUsing<PagedDocumentListToDocumentsVmConverter>();
            CreateMap<List<LinkedAccountsVm>, MyDocumentsVm>().ConvertUsing<LinkedAccountsVmToMyDocumentsVm>();
            CreateMap<FilterDocumentsVm, MyDocumentsVm>().ConvertUsing<FilterDocumentsVmToMyDocumentsVmConverter>();

            CreateMapsForModelsAndViewModels();
        }

        private void CreateMapsForModelsAndViewModels()
        {
            CreateMap<IncomeAndExpenditure, IncomeAndExpenditureApiModel>()
                .ConvertUsing<IncomeAndExpenditureServiceToApiConverter>();
            CreateMap<IncomeAndExpenditureApiModel, IncomeAndExpenditure>()
                .ConvertUsing<IncomeAndExpenditureApiToServiceConverter>();

            CreateMap<IncomeAndExpenditure, HouseholdStatusVm>()
                .ConvertUsing<IncomeAndExpenditureToHouseholdStatusVmConverter>();
            CreateMap<HouseholdStatusVm, IncomeAndExpenditure>()
                .ConvertUsing<HouseholdStatusVmToIncomeAndExpenditureConverter>();

            CreateMap<IncomeAndExpenditure, IncomeVm>()
                .ConvertUsing<IncomeAndExpenditureToIncomeVmConverter>();
            CreateMap<IncomeVm, IncomeAndExpenditure>()
                .ConvertUsing<IncomeVmToIncomeAndExpenditureConverter>();

            CreateMap<IncomeAndExpenditure, BillsAndOutgoingsVm>()
                .ConvertUsing<IncomeAndExpenditureToBillsAndOutgoingsVmConverter>();
            CreateMap<BillsAndOutgoingsVm, IncomeAndExpenditure>()
                .ConvertUsing<BillsAndOutgoingsVmToIncomeAndExpenditureConverter>();

            CreateMap<IncomeAndExpenditure, ExpendituresVm>()
                .ConvertUsing<IncomeAndExpenditureToExpendituresVmConverter>();
            CreateMap<ExpendituresVm, IncomeAndExpenditure>()
                .ConvertUsing<ExpendituresVmToIncomeAndExpenditureConverter>();

            CreateMap<ExpenditureMetricsApiModel, ExpenditureMetrics>()
                .ConvertUsing<ExpenditureMetricsApiToServiceConverter>();

            CreateMap<ExpenditureMetricApiModel, ExpenditureMetric>().ReverseMap();
            CreateMap<SaveOtherDebtsApiModel, SaveOtherDebts>().ReverseMap();
            CreateMap<MonthlyIncomeVm, MonthlyIncome>().ReverseMap();
            CreateMap<MonthlyOutgoingsVm, MonthlyOutgoings>().ReverseMap();
            CreateMap<BudgetSummaryVm, BudgetSummary>().ReverseMap();

            CreateMap<OpenWrksInvitationRequest, OpenWrksApiInvitationRequest>();
            CreateMap<OpenWrksApiInvitationResponse, OpenWrksInvitationResponse>();

            CreateMap<AccountSummary, DocumentAccountsVm>()
               .ForMember(x => x.AccountName, opts => opts.MapFrom(source => source.OriginalCompany))
               .ForMember(x => x.AccountReference, opts => opts.MapFrom(source => source.AccountReference))
               .ForMember(x => x.Reference, opts => opts.MapFrom(source => source.AccountReference))
               .ForMember(x => x.OutstandingBalance, opts => opts.MapFrom(source => source.OutstandingBalance))
               .ForMember(x => x.DiscountedBalance, opts => opts.MapFrom(source => source.DiscountedBalance))
               .ForMember(x => x.UnreadDocuments, opts => opts.MapFrom(source => source.UnreadDocuments))
               .ReverseMap();

            CreateMap<DocumentItem, DocumentVm>()
                 .ForMember(x => x.DocumentId, opts => opts.MapFrom(source => source.Id))
                 .ForMember(x => x.Received, opts => opts.MapFrom(source => source.Received))
                 .ForMember(x => x.Read, opts => opts.MapFrom(source => source.Read))
                 .ForMember(x => x.Subject, opts => opts.MapFrom(source => source.Subject))
                 .ForMember(x => x.IsNewDocument, opts => opts.MapFrom(source => source.IsNewDocument))
                 .ForMember(x => x.IsCustomer, opts => opts.MapFrom(source => source.IsCustomer))
               .ReverseMap();
        }
    }
}
