using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Diagnostics.CodeAnalysis;
using Serilog.Events;

#pragma warning disable 1591

namespace AlfaBank.WebApi
{
    // ReSharper disable once ClassNeverInstantiated.Global
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost
                .CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) => { config.AddEnvironmentVariables("ALFABANK_"); })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    Log.Logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(hostingContext.Configuration)
                        .WriteTo.Console(
                            LogEventLevel.Warning,
                            "===> {Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] [{TraceIdentifier}] {Message}{NewLine}{Exception}")
                        .WriteTo.RollingFile("./logs/alfabank-service-api-{Hour}.log",
                            LogEventLevel.Debug,
                            "===> {Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] [{TraceIdentifier}] {Message}{NewLine}{Exception}")
                        .CreateLogger();
                })
                .UseStartup<Startup>()
                .UseSerilog();
        }
    }
}