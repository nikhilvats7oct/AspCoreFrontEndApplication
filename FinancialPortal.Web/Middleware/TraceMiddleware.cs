using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinancialPortal.Web.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace FinancialPortal.Web.Middleware
{
    public static class TraceMiddleware
    {
        public const string TraceHeaderName = "X-Trace-Id";

        public static void AddTracing(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                string traceId;

                if (!context.Request.Headers.ContainsKey(TraceHeaderName))
                {
                    traceId = Guid.NewGuid().ToString("N");

                    context.Request.Headers.Add(TraceHeaderName, traceId);
                }
                else
                {
                    traceId = context.Request.Headers[TraceHeaderName];
                }

                // Add Trace ID to response.
                context.Response.OnStarting(() =>
                {
                    if (!context.Response.Headers.ContainsKey(TraceHeaderName))
                    {
                        context.Response.Headers.Add(TraceHeaderName, traceId);
                    }

                    return Task.CompletedTask;
                });

                using (LogContext.PushProperty("OccurrenceId", traceId))
                {
                    await next.Invoke();
                }
            });
        }
    }
}
