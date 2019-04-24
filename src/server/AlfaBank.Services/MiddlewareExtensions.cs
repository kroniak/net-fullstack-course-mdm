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
        public static void AddAlfaBankServices(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddTransient<ICurrencyConverter, CurrencyConverter>();
            services.AddTransient<ICardChecker, CardChecker>();
            services.AddTransient<ICardService, CardService>();
            services.AddTransient<IBusinessLogicValidationService, BusinessLogicValidationService>();
            services.AddTransient<IDtoValidationService, DtoValidationService>();
            services.AddTransient<IBankService, BankService>();
            services.AddTransient<ICardRepository, CardRepository>();
            services.AddTransient<ITransactionRepository, TransactionRepository>();
            services.AddTransient<ICardNumberGenerator, AlfaCardNumberGenerator>();
            services.AddTransient<IDtoFactory<Card, CardGetDto>, CardGetDtoFactory>();
            services.AddTransient<IDtoFactory<Transaction, TransactionGetDto>, TransactionGetDtoFactory>();
        }

        public static void AddInMemoryUserStorage(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            var user = GenerateUser();

            services.AddSingleton<IUserRepository>(new InMemoryUserRepository(user));
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