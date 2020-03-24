using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FinancialPortal.Web.Middleware;
using Microsoft.AspNetCore.Http;

namespace FinancialPortal.Web.HttpHandlers
{
    public class TracingHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TracingHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private static string TraceHeaderName => TraceMiddleware.TraceHeaderName;

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (_httpContextAccessor.HttpContext.Request.Headers.ContainsKey(TraceHeaderName))
            {
                request.Headers.Add(TraceHeaderName,
                    _httpContextAccessor.HttpContext.Request.Headers[TraceHeaderName].ToString());
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}