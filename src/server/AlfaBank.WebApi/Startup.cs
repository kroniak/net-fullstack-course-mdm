using System.Diagnostics.CodeAnalysis;
using AlfaBank.Core.Models;
using AlfaBank.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using AlfaBank.WebApi.Middleware;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;

namespace AlfaBank.WebApi
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();
            services.AddAutoMapper(conf => conf.AddProfile<DomainToDtoProfile>());
            services.AddAlfaBankServices();
            services.AddInMemoryUserStorage();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            // Register DB
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<BlContext>
                (options => options.UseSqlServer(connection));
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Service API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //Для doker HEALTHCHECK CMD curl --fail http://localhost:5000/health || exit
            app.UseHealthChecks("/health");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseHttpStatusCodeExceptionMiddleware();
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            // http://localhost:54745/swagger/
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Service  API V1");
            });
            app.UseMvc();
        }
    }
}