using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace AlfaBank.WebApi.Middleware
{
#pragma warning disable 1591
    [ExcludeFromCodeCoverage]
    public static class LoggerExtensions
#pragma warning restore 1591
    {
        /// <summary>
        /// Log Warning model state to ILogger
        /// </summary>
        /// <param name="logger">Current ILogger</param>
        /// <param name="message">Your message</param>
        /// <param name="modelState">ModelState from controller</param>
        public static void LogStateWarning(this ILogger logger, string message, ModelStateDictionary modelState)
        {
            var modelStateJson = ModelStateToJson(modelState);
            logger.LogWarning($"{message} [ModelState]: {modelStateJson}");
        }

        /// <summary>
        /// Log Error model state to ILogger
        /// </summary>
        /// <param name="logger">Current ILogger</param>
        /// <param name="message">Your message</param>
        /// <param name="modelState">ModelState from controller</param>
        public static void LogStateError(this ILogger logger, string message, ModelStateDictionary modelState)
        {
            var modelStateJson = ModelStateToJson(modelState);
            logger.LogWarning($"{message} [ModelState]: {modelStateJson}");
        }

        private static string ModelStateToJson(ModelStateDictionary modelState)
        {
            var serializableModelState = new SerializableError(modelState);
            var modelStateJson = JsonConvert.SerializeObject(serializableModelState);
            return modelStateJson;
        }
    }
}