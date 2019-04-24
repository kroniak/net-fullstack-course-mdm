using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace AlfaBank.WebApi.Middleware
{
    [ExcludeFromCodeCoverage]
    public static class MiddlewareExtensions
    {
        // ReSharper disable once UnusedMethodReturnValue.Global
        public static IApplicationBuilder UseHttpStatusCodeExceptionMiddleware(this IApplicationBuilder builder)
            => builder.UseMiddleware<HttpStatusCodeExceptionMiddleware>();

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