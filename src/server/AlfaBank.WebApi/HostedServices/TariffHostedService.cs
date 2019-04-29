using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using AlfaBank.Core.Data.Interfaces;
using AlfaBank.Services;
using AlfaBank.Services.Checkers;
using AlfaBank.Services.Converters;
using AlfaBank.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// ReSharper disable ClassNeverInstantiated.Global

namespace AlfaBank.WebApi.HostedServices
{
    /// <summary>
    /// Background task service for tariff charging
    /// </summary>
    [ExcludeFromCodeCoverage]
    // ReSharper disable once InheritDocConsiderUsage
    public class TariffHostedService : IHostedService, IDisposable
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<TariffHostedService> _logger;
        private readonly IUserRepository _userRepository;
        private Timer _timer;

        /// <inheritdoc />
        public TariffHostedService(
            IServiceProvider services,
            ILogger<TariffHostedService> logger,
            IUserRepository userRepository
        )
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        /// <inheritdoc />
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Tariff Background Service is starting.");

            _timer = new Timer(BackgroundTask, new { }, TimeSpan.Zero, TimeSpan.FromSeconds(30));

            return Task.CompletedTask;
        }

        private void BackgroundTask(object state)
        {
            lock (state)
            {
                using (var scope = _services.CreateScope())
                {
                    var converter =
                        scope.ServiceProvider
                            .GetRequiredService<ICurrencyConverter>();

                    var checker =
                        scope.ServiceProvider
                            .GetRequiredService<ICardChecker>();

                    var repository =
                        scope.ServiceProvider
                            .GetRequiredService<ICardRepository>();

                    var service = new CardService(checker, converter);

                    TariffCharge(repository, service);
                }
            }
        }

        private void TariffCharge(ICardRepository repository, ICardService service)
        {
            _logger.LogInformation("Tariff Background Service is started process.");

            var user = _userRepository.GetCurrentUser();

            var cards = repository.All(user);

            foreach (var card in cards)
            {
                var result = service.TryTariffCharge(card, 0.01M);

                if (result)
                {
                    _logger.LogInformation($"Tariff Background Service is successfully for card {card.CardNumber}.");
                }
                else
                {
                    _logger.LogError($"Tariff Background Service is error for card {card.CardNumber}.");
                }
            }
        }

        /// <inheritdoc />
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Tariff Background Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public void Dispose() => _timer?.Dispose();
    }
}