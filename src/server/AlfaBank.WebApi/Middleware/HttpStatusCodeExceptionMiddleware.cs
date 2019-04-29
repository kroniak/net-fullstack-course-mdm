using AlfaBank.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Events;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

#pragma warning disable 1591

namespace AlfaBank.WebApi.Middleware
{
    // ReSharper disable once ClassNeverInstantiated.Global
    [ExcludeFromCodeCoverage]
    public class HttpStatusCodeExceptionMiddleware
    {
        private const string MessageTemplate =
            "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";

        private static readonly ILogger Logger = Log.ForContext<HttpStatusCodeExceptionMiddleware>();
        private readonly RequestDelegate _next;

        public HttpStatusCodeExceptionMiddleware(RequestDelegate next)
        {
            _next = next ??
                    throw new ArgumentNullException(nameof(next));
        }

        // ReSharper disable once UnusedMember.Global
        public async Task Invoke(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var sw = Stopwatch.StartNew();

            try
            {
                await _next(context);
                sw.Stop();

                var statusCode = context.Response?.StatusCode;
                var level = statusCode > 499 ? LogEventLevel.Error : LogEventLevel.Information;

                var logger = level == LogEventLevel.Error ? LogForErrorContext(context) : Logger;
                logger.Write(
                    level,
                    MessageTemplate,
                    context.Request.Method,
                    context.Request.Path,
                    statusCode,
                    sw.Elapsed.TotalMilliseconds);
            }
            catch (CriticalException ex) when (LogException(context, sw, ex))
            {
                var code = (int) ex.StatusCode;

                if (context.Response != null && !context.Response.HasStarted)
                {
                    context.Response.Clear();
                    context.Response.StatusCode = code;
                }

                await context.WriteErrorAsync(code, "500 Critical server error - " + ex.Message);
            }
            catch (Exception ex) when (LogException(context, sw, ex))
            {
                const int code = (int) HttpStatusCode.InternalServerError;

                if (context.Response != null && !context.Response.HasStarted)
                {
                    context.Response.Clear();
                    context.Response.StatusCode = code;
                }

                await context.WriteErrorAsync(code, "500 Critical server error - " + ex.Message);
            }
        }

        private static bool LogException(HttpContext httpContext, Stopwatch sw, Exception ex)
        {
            sw.Stop();

            LogForErrorContext(httpContext)
                .Error(
                    ex,
                    MessageTemplate,
                    httpContext.Request.Method,
                    httpContext.Request.Path,
                    500,
                    sw.Elapsed.TotalMilliseconds);

            return false;
        }

        private static ILogger LogForErrorContext(HttpContext httpContext)
        {
            var request = httpContext.Request;

            var result = Log
                .ForContext(
                    "RequestHeaders",
                    request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()),
                    true)
                .ForContext("RequestHost", request.Host)
                .ForContext("RequestProtocol", request.Protocol);

            if (request.HasFormContentType)
            {
                result = result.ForContext(
                    "RequestForm",
                    request.Form.ToDictionary(v => v.Key, v => v.Value.ToString()));
            }

            return result;
        }
    }
}