using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using AlfaBank.Core.Exceptions;
using Microsoft.AspNetCore.Http;

namespace AlfaBank.WebApi.Middleware
{
    // ReSharper disable once ClassNeverInstantiated.Global
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [ExcludeFromCodeCoverage]
    public class HttpStatusCodeExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public HttpStatusCodeExceptionMiddleware(RequestDelegate next)
        {
            _next = next ??
                    throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (CriticalException ex)
            {
                var code = (int) ex.StatusCode;

                if (!context.Response.HasStarted)
                {
                    context.Response.Clear();
                    context.Response.StatusCode = code;
                }

                await context.WriteErrorAsync(code, "500 Critical server error - " + ex.Message);
            }
            catch (Exception ex)
            {
                const int code = (int) HttpStatusCode.InternalServerError;

                if (!context.Response.HasStarted)
                {
                    context.Response.Clear();
                    context.Response.StatusCode = code;
                }

                await context.WriteErrorAsync(code, "500 Critical server error - " + ex.Message);
            }
        }
    }
}