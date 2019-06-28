using AlfaBank.Core.Models;
using AlfaBank.Services;
using AlfaBank.WebApi.HealthCheckers;
using AlfaBank.WebApi.Middleware;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Core.Enrichers;
using Serilog.Enrichers.AspNetCore.HttpContext;
using System;
using System.Diagnostics.CodeAnalysis;
using AlfaBank.Core.Data;
using AlfaBank.WebApi.HostedServices;
using AlfaBank.WebApi.Services;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

// ReSharper disable UnusedMember.Global

#pragma warning disable 1591

namespace AlfaBank.WebApi
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly ILoggerFactory _loggerFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The current configuration.</param>
        /// <param name="loggerFactory">Current logger factory</param>
        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        /// <summary>
        /// Configures services for the application.
        /// </summary>
        /// <param name="services">The collection of services to configure the application with.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(options => options.AddProfile<DomainToDtoProfile>());

            // Add db
            services.AddDbContext<SqlContext>(options =>
            {
                options.UseLoggerFactory(_loggerFactory);
                options.UseSqlServer(
                    _configuration.GetConnectionString("DB"),
                    b => b.MigrationsAssembly("AlfaBank.WebApi"));
            });

            // Add owns services
            services.AddMemoryCache();
            services.AddAlfaBankServices();
            services.AddScoped<ISimpleAuthenticateService, SimpleAuthenticateService>();

            // Add Background service
            services.AddHostedService<TariffHostedService>();

            // Add MVC and other services
            services.AddMvc(options => options.EnableEndpointRouting = true)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwagger();

            // Add custom HealthCheck and UI
            services
                .AddCustomHealthChecks()
                .AddElkHealthChecks(_configuration);

            services.AddCors(options =>
            {
                options.AddPolicy("AllowMyOrigin",
                    builder =>
                        builder
                            .WithOrigins("http://localhost:3000")
                            .AllowAnyMethod()
                            .AllowAnyHeader());
            });

            services.AddCustomAuthentication(_configuration);
        }

        /// <summary>
        /// Configures the application using the provided builder, hosting environment, and API version description provider.
        /// </summary>
        /// <param name="app">The current application builder.</param>
        /// <param name="env">The current hosting environment.</param>
        /// <param name="provider">The API version descriptor provider used to enumerate defined API versions.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider provider)
        {
            app.UseSerilogLogContext(options =>
            {
                options.EnrichersForContextFactory = context => new[]
                {
                    // TraceIdentifier property will be available in all chained middlewares. And yes - it is HttpContext specific
                    new PropertyEnricher("TraceIdentifier", context.TraceIdentifier)
                };
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpStatusCodeExceptionMiddleware();

            app.UseSwagger();

            app.UseSwaggerUI(
                options =>
                {
                    // build a swagger endpoint for each discovered API version
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());
                    }
                });

            app.UseCustomHealthCheckEndpoints();

            app.UseAuthentication();

            app.UseCors("AllowMyOrigin");
            app.UseMvc();
        }
    }
}