using AlfaBank.Core.Data.Interfaces;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable ClassNeverInstantiated.Global
namespace AlfaBank.WebApi.HealthCheckers
{
    /// <inheritdoc />
    /// <summary>
    /// Custom check fro count transaction in storage
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CardsCountHealthCheck : IHealthCheck
    {
        private readonly ICardRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CardsCountHealthCheck"/> class.
        /// <param name="repository">ICardRepository instance</param>
        /// </summary>
        public CardsCountHealthCheck(ICardRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <inheritdoc />
        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var count = _repository.Count();

            if (count > 0)
            {
                return Task.FromResult(
                    HealthCheckResult.Healthy(
                        data:
                        new Dictionary<string, object>
                        {
                            {"count", count}
                        }));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("No cards in repository"));
        }
    }
}