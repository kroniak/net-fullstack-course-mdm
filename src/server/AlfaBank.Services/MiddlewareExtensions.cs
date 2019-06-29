using System;
using System.Diagnostics.CodeAnalysis;
using AlfaBank.Core.Data.Interfaces;
using AlfaBank.Core.Data.Repositories;
using AlfaBank.Core.Models;
using AlfaBank.Core.Models.Dto;
using AlfaBank.Core.Models.Factories;
using AlfaBank.Services.Checkers;
using AlfaBank.Services.Converters;
using AlfaBank.Services.Generators;
using AlfaBank.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedMember.Global

namespace AlfaBank.Services
{
    [ExcludeFromCodeCoverage]
    public static class MiddlewareExtensions
    {
        public static IServiceCollection AddAlfaBankServices(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services
                .AddSingleton<ICurrencyConverter, CurrencyConverter>()
                .AddSingleton<ICardChecker, CardChecker>()
                .AddSingleton<ICardNumberGenerator, AlfaCardNumberGenerator>()
                .AddSingleton<IDtoValidationService, DtoValidationService>()
                .AddSingleton<IBusinessLogicValidationService, BusinessLogicValidationService>()
                .AddScoped<IDtoFactory<Card, CardGetDto>, CardGetDtoFactory>()
                .AddScoped<IDtoFactory<Transaction, TransactionGetDto>, TransactionGetDtoFactory>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<ICardRepository, CardRepository>()
                .AddScoped<ITransactionRepository, TransactionRepository>()
                .AddScoped<ICardService, CardService>()
                .AddScoped<IBankService, BankService>();

            return services;
        }
    }
}