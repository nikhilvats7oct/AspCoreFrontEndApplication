using AutoMapper;
using FinancialPortal.Web.DataTransferObjects;
using FinancialPortal.Web.Services.ApiModels;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Settings;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Services
{
    public class DocumentsService : IDocumentsService
    {
        private readonly ILogger<DocumentsService> _logger;
        private readonly IRestClient _restClient;
        private readonly PortalSetting _portalSettings;
        private readonly IMapper _mapper;

        public DocumentsService(ILogger<DocumentsService> logger,
                                IRestClient restClient,
                                PortalSetting portalSettings,
                                IMapper mapper)
        {
            _logger = logger;
            _restClient = restClient;
            _portalSettings = portalSettings;
            _mapper = mapper;
        }

        public async Task<List<DocumentItem>> GetMyDocuments(string accountReference)
        {
            var url = $"{_portalSettings.GatewayEndpoint}api/Documents/GetDocuments";

            GetDocumentsDto dto = new GetDocumentsDto()
            {
                AccountId = accountReference
            };

            var documents = await _restClient.PostAsync<GetDocumentsDto, List<DocumentItem>>(url, dto);

            return documents;
        }

        public async Task UpdateDocument(string accountReference, int documentId)
        {
            var url = $"{_portalSettings.GatewayEndpoint}api/Documents/UpdateDocument";

            var date = DateTime.UtcNow.ToString("s") + "Z";

            UpdateDocumentDto dto = new UpdateDocumentDto()
            {
                AccountId = accountReference,
                DocumentId = documentId,
                Read = date
            };

            await _restClient.PostNoResponseAsync(url, dto);
        }

        public async Task<Stream> DownloadDocument(string accountReference, int documentId)
        {
            var url = $"{_portalSettings.GatewayEndpoint}api/Documents/GetPdf";

            GetPdfDto dto = new GetPdfDto()
            {
                AccountId = accountReference,
                DocumentId = documentId
            };

            var pdf = await _restClient.PostStreamAsync(url, dto);

            return pdf;
        }
    }
}
