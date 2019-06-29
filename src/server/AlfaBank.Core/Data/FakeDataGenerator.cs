using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AlfaBank.Core.Infrastructure;
using AlfaBank.Core.Models;
using Microsoft.AspNetCore.Identity;

// ReSharper disable LoopCanBeConvertedToQuery

namespace AlfaBank.Core.Data
{
    [ExcludeFromCodeCoverage]
    public static class FakeDataGenerator
    {
        /// <summary>
        /// Generate Fake users for init db
        /// </summary>
        /// <param name="userNames"></param>
        /// <returns>Enumerable of Fake users</returns>
        public static IEnumerable<User> GenerateFakeUsers(IEnumerable<string> userNames) =>
            userNames.Select((name, i) =>
            {
                const string password = "12345678";
                var user = new User(name, "init password") {Id = i + 1};
                var hashedPassword = new PasswordHasher<User>().HashPassword(user, password);

                user.Password = hashedPassword;
                return user;
            });

        /// <summary>
        /// Generate Fake cards for init db
        /// </summary>
        /// <param name="users">List of the Users</param>
        /// <returns>Enumerable of Fake Cards</returns>
        public static IEnumerable<Card> GenerateFakeCards(IEnumerable<User> users)
        {
            var date = DateTime.Today.AddYears(-2);

            // create fake cards
            var cards = new List<Card>();
            var id = 1;

            foreach (var user in users)
            {
                var cardsForUser = new List<Card>
                {
                    new Card
                    {
                        Id = id++,
                        UserId = user.Id,
                        CardNumber = AlfaCardNumberGenerator.GenerateNewCardNumber(CardType.VISA),
                        CardName = "my salary",
                        Currency = Currency.RUR,
                        CardType = CardType.VISA,
                        DtOpenCard = date
                    },
                    new Card
                    {
                        Id = id++,
                        UserId = user.Id,
                        CardNumber = AlfaCardNumberGenerator.GenerateNewCardNumber(CardType.MAESTRO),
                        CardName = "my salary",
                        Currency = Currency.RUR,
                        CardType = CardType.MAESTRO,
                        DtOpenCard = date
                    },
                    new Card
                    {
                        Id = id++,
                        UserId = user.Id,
                        CardNumber = AlfaCardNumberGenerator.GenerateNewCardNumber(CardType.VISA),
                        CardName = "my debt",
                        Currency = Currency.EUR,
                        CardType = CardType.VISA,
                        DtOpenCard = date
                    },
                    new Card
                    {
                        Id = id++,
                        UserId = user.Id,
                        CardNumber = AlfaCardNumberGenerator.GenerateNewCardNumber(CardType.MASTERCARD),
                        CardName = "for my family",
                        Currency = Currency.USD,
                        CardType = CardType.MASTERCARD,
                        DtOpenCard = date
                    }
                };

                cards.AddRange(cardsForUser);
            }

            return cards;
        }

        /// <summary>
        /// Generate Fake transactions for init db
        /// </summary>
        /// <param name="cards"></param>
        /// <returns>Enumerable of Fake transactions</returns>
        public static IEnumerable<Transaction> GenerateFakeTransactions(IEnumerable<Card> cards) =>
            cards.Select((card, i) => new Transaction
            {
                Id = i + 1,
                CardId = card.Id,
                CardToNumber = card.CardNumber,
                Sum = GetConvertedSum(10M, Currency.RUR, card.Currency)
            });

        private static decimal GetConvertedSum(decimal sum, Currency from, Currency to)
        {
            if (from == to) return sum;

            return sum * Constants.Currencies[from] / Constants.Currencies[to];
        }
    }
}