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
    public class TransactionsCountHealthCheck : IHealthCheck
    {
        private readonly ITransactionRepository _repository;
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionsCountHealthCheck"/> class.
        /// Custom check fro count transaction in storage
        /// <param name="repository">ITransactionRepository instance</param>
        /// <param name="userRepository">IUserRepository instance</param>
        /// </summary>
        public TransactionsCountHealthCheck(ITransactionRepository repository, IUserRepository userRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(_userRepository));
        }

        /// <inheritdoc />
        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var count = _repository.CountLastHour(_userRepository.GetCurrentUser());

            if (count > 1)
            {
                return Task.FromResult(
                    HealthCheckResult.Healthy(
                        data:
                        new Dictionary<string, object>
                        {
                            { "count", count }
                        }));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("No transaction at last hour"));
        }
    }
}