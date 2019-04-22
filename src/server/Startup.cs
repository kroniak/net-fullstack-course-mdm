using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Server.Data;
using Server.Data.Interfaces;
using Server.Middleware;
using Server.Models;
using Server.Models.Dto;
using Server.Models.Factories;
using Server.Services;
using Server.Services.Checkers;
using Server.Services.Converters;
using Server.Services.Generators;
using Server.Services.Interfaces;

namespace Server
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureOwnServices(services);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        private static void ConfigureOwnServices(IServiceCollection services)
        {
            services.AddTransient<ICurrencyConverter, CurrencyConverter>();
            services.AddTransient<ICardChecker, CardChecker>();
            services.AddTransient<ICardService, CardService>();
            services.AddTransient<IBusinessLogicValidationService, BusinessLogicValidationService>();
            services.AddTransient<IDtoValidationService, DtoValidationService>();
            services.AddTransient<IBankService, BankService>();
            services.AddTransient<IFakeDataGenerator, FakeDataGenerator>();
            services.AddTransient<ICardRepository, CardRepository>();
            services.AddTransient<ITransactionRepository, TransactionRepository>();
            services.AddTransient<ICardNumberGenerator, AlfaCardNumberGenerator>();
            services.AddTransient<IDtoFactory<Card, CardGetDto>, CardGetDtoFactory>();
            services.AddTransient<IDtoFactory<Transaction, TransactionGetDto>, TransactionGetDtoFactory>();
            services.AddSingleton<IUserRepository, InMemoryUserRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<HttpStatusCodeExceptionMiddleware>();

            app.UseMvc();
        }
    }
}