using System;
using System.Diagnostics.CodeAnalysis;
using AlfaBank.Core.Data;
using AlfaBank.Core.Data.Interfaces;
using AlfaBank.Core.Models;
using AlfaBank.Core.Models.Dto;
using AlfaBank.Core.Models.Factories;
using AlfaBank.Services.Checkers;
using AlfaBank.Services.Converters;
using AlfaBank.Services.Generators;
using AlfaBank.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

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
                .AddTransient<ICurrencyConverter, CurrencyConverter>()
                .AddTransient<ICardChecker, CardChecker>()
                .AddTransient<ICardService, CardService>()
                .AddTransient<IBusinessLogicValidationService, BusinessLogicValidationService>()
                .AddTransient<IDtoValidationService, DtoValidationService>()
                .AddTransient<IBankService, BankService>()
                .AddTransient<ICardRepository, CardRepository>()
                .AddTransient<ITransactionRepository, TransactionRepository>()
                .AddTransient<ICardNumberGenerator, AlfaCardNumberGenerator>()
                .AddTransient<IDtoFactory<Card, CardGetDto>, CardGetDtoFactory>()
                .AddTransient<IDtoFactory<Transaction, TransactionGetDto>, TransactionGetDtoFactory>();

            return services;
        }

        public static IServiceCollection AddInMemoryUserStorage(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            var user = GenerateUser();

            services.AddSingleton<IUserRepository>(new InMemoryUserRepository(user));

            return services;
        }

        /// <summary>
        /// Generate test user
        /// </summary>
        /// <returns> Test user</returns>
        private static User GenerateUser()
        {
            var fakeDataGenerator = new FakeDataGenerator(new CardService(new CardChecker(),
                new CurrencyConverter()), new AlfaCardNumberGenerator());

            var cards = fakeDataGenerator.GenerateFakeCards();
            var user = fakeDataGenerator.GenerateFakeUser();
            user.Cards.AddRange(cards);
            return user;
        }
    }
}