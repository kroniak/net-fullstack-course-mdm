using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AlfaBank.WebApi.Middleware
{
    /// <summary>
    /// Extension to adding custom IApplicationBuilder middlewares
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class MiddlewareExtensions
    {
        // ReSharper disable once UnusedMethodReturnValue.Global
        /// <summary>
        /// Extensions for user custom `HttpStatusCodeExceptionMiddleware` middleware
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <returns>IApplicationBuilder instance</returns>
        public static IApplicationBuilder UseHttpStatusCodeExceptionMiddleware(this IApplicationBuilder app)
            => app.UseMiddleware<HttpStatusCodeExceptionMiddleware>();

        /// <summary>
        /// Methods to writing errors to context response
        /// </summary>
        /// <param name="context">Current context</param>
        /// <param name="statusCode">Current error status code</param>
        /// <param name="message">Current error message</param>
        /// <returns>Task type for WriteAsync</returns>
        public static Task WriteErrorAsync(this HttpContext context, int statusCode, string message)
        {
            var error = new
            {
                httpStatusCode = statusCode,
                errorMessage = message
            };
            var json = JsonConvert.SerializeObject(error);

            return context.Response.WriteAsync(json);
        }

        /// <summary>
        /// Add swagger and versioning middleware and services
        /// </summary>
        /// <param name="services"></param>
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddApiVersioning(
                options =>
                {
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                    // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                    options.ReportApiVersions = true;
                });

            services.AddVersionedApiExplorer(
                options =>
                {
                    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                    // note: the specified format code will format the version as "'v'major[.minor][-status]"
                    options.GroupNameFormat = "'v'VVV";

                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.SubstituteApiVersionInUrl = true;
                });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddSwaggerGen(
                options =>
                {
                    // add a custom operation filter which sets default values
                    options.OperationFilter<SwaggerDefaultValues>();

                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    // integrate xml comments
                    options.IncludeXmlComments(xmlPath);
                });
        }

        /// <summary>
        /// Add custom Auth middleware
        /// </summary>
        /// <param name="services">IService Collections</param>
        /// <param name="configuration">Global configuration</param>
        public static void AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var key = configuration.GetSection("Auth").GetValue<string>("Key");

            var keyEncoded = Encoding.ASCII.GetBytes(key);
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(keyEncoded),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }
    }
}