using AlfaBank.Services;
using AlfaBank.WebApi;
using AlfaBank.WebApi.HealthCheckers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Server.Test.HealthCheckers
{
    public class TransactionsCountHealthCheckTest
    {
        [Fact]
        public void AddHealthCheck_Properly_Configured()
        {
            // Arrange
            var services = new ServiceCollection();
            services
                .AddInMemoryUserStorage()
                .AddAlfaBankServices()
                .AddHealthChecks()
                .AddCheck<TransactionsCountHealthCheck>(
                    "transactions-count-by-hour",
                    HealthStatus.Degraded,
                    new[] { "transactions" });

            // Act
            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<HealthCheckServiceOptions>>();
            var registration = options.Value.Registrations.First();

            // Assert
            Assert.Equal("transactions-count-by-hour", registration.Name);
        }

        [Fact]
        public async Task WebHostBuilder_InitialStateCards_Healthy()
        {
            // Arrange
            var webHostBuilder = new WebHostBuilder()
                .UseStartup<Startup>()
                .ConfigureServices(
                    services =>
                    {
                        services
                            .AddInMemoryUserStorage()
                            .AddAlfaBankServices()
                            .AddHealthChecks()
                            .AddCheck<TransactionsCountHealthCheck>(
                                "transactions-count-by-hour",
                                HealthStatus.Degraded,
                                new[] { "transactions" });
                    })
                .Configure(
                    app =>
                    {
                        app.UseHealthChecks(
                            "/health",
                            new HealthCheckOptions
                            {
                                Predicate = r => r.Tags.Contains("transactions")
                            });
                    });

            var server = new TestServer(webHostBuilder);

            // Act
            var response = await server.CreateRequest("/health")
                .GetAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}