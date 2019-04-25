using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace AlfaBank.WebApi
{
    // ReSharper disable once ClassNeverInstantiated.Global
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                // -- This configuration (and Environment and Json) and  was enabled automatically 
                //.AddCommandLine(args) 
                .Build();

            return WebHost
                .CreateDefaultBuilder(args)
                .UseConfiguration(configuration)
                .UseStartup<Startup>();
        }
    }
}