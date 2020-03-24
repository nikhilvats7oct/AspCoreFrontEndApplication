using FinancialPortal.Web.Services.ApiModels;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Services.Interfaces
{
    public interface IDocumentsService
    {
        Task<List<DocumentItem>> GetMyDocuments(string accountReference);
        Task UpdateDocument(string accountReference, int documentId);
        Task<Stream> DownloadDocument(string accountReference, int documentId);
    }
}
