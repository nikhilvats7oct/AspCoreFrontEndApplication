using System.Threading.Tasks;

namespace FinancialPortal.Web.Services.Interfaces
{
    public interface IUnreadDocumentsService
    {
        Task<bool> HasUnreadDocuments();
    }
}