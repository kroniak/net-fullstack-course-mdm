using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Server.Exceptions;

namespace Server.Middleware
{
    // ReSharper disable once ClassNeverInstantiated.Global
    [ExcludeFromCodeCoverage]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
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

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class HttpStatusCodeExceptionMiddlewareExtensions
    {
        // ReSharper disable once UnusedMethodReturnValue.Global

        public static async Task WriteErrorAsync(this HttpContext context, int statusCode, string message)
        {
            var error = new
            {
                httpStatusCode = statusCode,
                errorMessage = message
            };
            var json = JsonConvert.SerializeObject(error);

            await context.Response.WriteAsync(json);
        }
    }
}