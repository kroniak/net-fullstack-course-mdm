using System.Diagnostics.CodeAnalysis;
using AlfaBank.Core.Models;
using AlfaBank.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using AlfaBank.WebApi.Middleware;
using AutoMapper;

namespace AlfaBank.WebApi
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(conf => conf.AddProfile<DomainToDtoProfile>());
            services.AddAlfaBankServices();
            services.AddInMemoryUserStorage();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseHttpStatusCodeExceptionMiddleware();

            app.UseMvc();
        }
    }
}