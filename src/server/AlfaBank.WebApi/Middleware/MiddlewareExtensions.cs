using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace AlfaBank.WebApi.Middleware
{
    /// <summary>
    /// Extension to adding custom IApplicationBuilder middlewares
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class MiddlewareExtensions
    {
        // ReSharper disable once UnusedMethodReturnValue.Global

        /// <summary>
        /// Extensions for user custom `HttpStatusCodeExceptionMiddleware` middleware
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <returns>IApplicationBuilder instance</returns>
        public static IApplicationBuilder UseHttpStatusCodeExceptionMiddleware(this IApplicationBuilder app)
            => app.UseMiddleware<HttpStatusCodeExceptionMiddleware>();

        /// <summary>
        /// Methods to writing errors to context response
        /// </summary>
        /// <param name="context">Current context</param>
        /// <param name="statusCode">Current error status code</param>
        /// <param name="message">Current error message</param>
        /// <returns>Task type for WriteAsync</returns>
        public static Task WriteErrorAsync(this HttpContext context, int statusCode, string message)
        {
            var error = new
            {
                httpStatusCode = statusCode,
                errorMessage = message
            };
            var json = JsonConvert.SerializeObject(error);

            return context.Response.WriteAsync(json);
        }
    }
}