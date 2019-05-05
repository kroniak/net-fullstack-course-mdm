using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AlfaBank.WebApi.HealthCheckers
{
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MemoryHealthCheck : IHealthCheck
    {
        /// <inheritdoc />
        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            const long threshold = 1 * 1024 * 1024 * 1024;

            // Include GC information in the reported diagnostics.
            var allocated = GC.GetTotalMemory(false);
            var data = new Dictionary<string, object>
            {
                {"AllocatedBytes", allocated},
                {"AllocatedKBytes", allocated / 1024},
                {"AllocatedMBytes", allocated / 1024 / 1024},
                {"AllocatedGBytes", allocated / 1024 / 1024 / 1024},
                {"Gen0Collections", GC.CollectionCount(0)},
                {"Gen1Collections", GC.CollectionCount(1)},
                {"Gen2Collections", GC.CollectionCount(2)}
            };

            var status = allocated < threshold ? HealthStatus.Healthy : HealthStatus.Unhealthy;

            return Task.FromResult(new HealthCheckResult(
                status,
                "Reports degraded status if allocated bytes " +
                $">= {threshold} bytes.",
                null,
                data));
        }
    }
}