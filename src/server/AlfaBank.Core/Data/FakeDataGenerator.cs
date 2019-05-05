using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AlfaBank.Core.Infrastructure;
using AlfaBank.Core.Models;

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
            userNames.Select((name, i) => new User(name) {Id = i + 1});

        /// <summary>
        /// Generate Fake cards for init db
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Enumerable of Fake Cards</returns>
        public static IEnumerable<Card> GenerateFakeCards(User user)
        {
            var date = DateTime.Today.AddYears(-2);
            // create fake cards
            var cards = new List<Card>
            {
                new Card
                {
                    Id = 1,
                    UserId = user.Id,
                    CardNumber = "6271190189011743",
                    CardName = "my salary",
                    Currency = Currency.RUR,
                    CardType = CardType.VISA,
                    DtOpenCard = date
                },
                new Card
                {
                    Id = 2,
                    UserId = user.Id,
                    CardNumber = "6762302693240520",
                    CardName = "my salary",
                    Currency = Currency.RUR,
                    CardType = CardType.MAESTRO,
                    DtOpenCard = date
                },
                new Card
                {
                    Id = 3,
                    UserId = user.Id,
                    CardNumber = "4083967629457310",
                    CardName = "my debt",
                    Currency = Currency.EUR,
                    CardType = CardType.VISA,
                    DtOpenCard = date
                },
                new Card
                {
                    Id = 4,
                    UserId = user.Id,
                    CardNumber = "5101265622568232",
                    CardName = "for my lovely wife",
                    Currency = Currency.USD,
                    CardType = CardType.MASTERCARD,
                    DtOpenCard = date
                }
            };

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