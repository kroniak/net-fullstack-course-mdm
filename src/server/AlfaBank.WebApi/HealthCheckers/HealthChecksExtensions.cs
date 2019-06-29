using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// ReSharper disable UnusedMethodReturnValue.Global

namespace AlfaBank.WebApi.HealthCheckers
{
    /// <summary>
    /// Extensions to adding custom HealthCheckers
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class HealthChecksExtensions
    {
        /// <summary>
        /// Add custom UI and JSON endpoints to pipeline
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <returns>IApplicationBuilder instance</returns>
        public static IApplicationBuilder UseCustomHealthCheckEndpoints(this IApplicationBuilder app)
        {
            app
                .UseHealthChecks(
                    "/health/detail",
                    new HealthCheckOptions
                    {
                        Predicate = _ => true,
                        AllowCachingResponses = false,
                        ResponseWriter = WriteResponse
                    })
                .UseHealthChecks("/health/ready", new HealthCheckOptions
                {
                    Predicate = check => check.Tags.Contains("ready"),
                    AllowCachingResponses = false
                })
                .UseHealthChecks("/health/live", new HealthCheckOptions
                {
                    // Exclude all checks and return a 200-Ok
                    Predicate = _ => false,
                    AllowCachingResponses = false
                });

            return app;
        }

        /// <summary>
        /// Add custom Health Checkers for our services
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <returns>IHealthChecksBuilder instance</returns>
        public static IHealthChecksBuilder AddCustomHealthChecks(this IServiceCollection services)
        {
            return services
                .AddHealthChecks()
                .AddCheck<MemoryHealthCheck>(
                    "memory-check",
                    HealthStatus.Degraded,
                    new[] {"memory"})
                .AddCheck<TransactionsCountHealthCheck>(
                    "transactions-count-by-hour",
                    HealthStatus.Degraded,
                    new[] {"transactions"})
                .AddCheck<CardsCountHealthCheck>(
                    "cards-count",
                    HealthStatus.Degraded,
                    new[] {"cards"})
                .AddUrlGroup(
                    option =>
                    {
                        option.AddUri(
                            new Uri("http://google.com"),
                            setup =>
                            {
                                setup.UseGet();
                                setup.ExpectHttpCode(200);
                            });
                    },
                    "network-access",
                    HealthStatus.Degraded,
                    new[] {"ready", "network"});
        }

        /// <summary>
        /// Add custom ELK Health Checker
        /// </summary>
        /// <param name="builder">IHealthChecksBuilder</param>
        /// <param name="configuration">Current configuration root</param>
        /// <returns>IHealthChecksBuilder instance</returns>
        public static IHealthChecksBuilder AddElkHealthChecks(
            this IHealthChecksBuilder builder,
            IConfiguration configuration)
        {
            return builder
                .AddElasticsearch(
                    configuration.GetConnectionString("ELK"),
                    "elasticsearch",
                    HealthStatus.Degraded,
                    new[] {"ready", "logs"});
        }

        private static Task WriteResponse(HttpContext httpContext,
            HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";

            var json = new JObject(
                new JProperty("status", result.Status.ToString()),
                new JProperty("results", new JObject(result.Entries.Select(pair =>
                    new JProperty(pair.Key, new JObject(
                        new JProperty("status", pair.Value.Status.ToString()),
                        new JProperty("description", pair.Value.Description),
                        new JProperty("data", new JObject(pair.Value.Data.Select(
                            p => new JProperty(p.Key, p.Value))))))))));
            return httpContext.Response.WriteAsync(
                json.ToString(Formatting.Indented));
        }
    }
}