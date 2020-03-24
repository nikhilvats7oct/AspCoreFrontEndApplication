using System;
using System.IO;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Services.Interfaces
{
    public interface IRestClient : IDisposable
    {
        Task<TResponse> GetAsync<TResponse>(string uri);

        Task<TResponse> PostAsync<TRequest, TResponse>(string uri, TRequest request);

        Task PostNoResponseAsync<TRequest>(string uri, TRequest request);

        Task<Stream> PostStreamAsync<TRequest>(string uri, TRequest request);
    }
}