using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Proxy
{
    public class RestException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Content { get; set; }
        public Guid? ExceptionOccurrenceId { get; set; }

        public RestException(HttpStatusCode statusCode, string content)
            : base($"API call failed: {(int)statusCode} - {Enum.GetName(typeof(HttpStatusCode), statusCode)}, content: {content}")
        {
            StatusCode = statusCode;
            Content = content;

            Debug.Assert(Guid.TryParse(null, out var test) == false); // ensure no exception if content is null

            // If contains a GUID, indicates that this is an id that can be used to correlate logs
            if (Guid.TryParse(content, out var contentGuid))
            {
                ExceptionOccurrenceId = contentGuid;
            }
        }
    }
}
