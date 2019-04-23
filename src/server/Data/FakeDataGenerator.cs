using System;
using System.Collections.Generic;
using Server.Data.Interfaces;
using Server.Infrastructure;
using Server.Models;
using Server.Services.Interfaces;

namespace Server.Data
{
    public class FakeDataGenerator : IFakeDataGenerator
    {
        private readonly ICardService _cardService;
        private readonly ICardNumberGenerator _cardNumberGenerator;

        public FakeDataGenerator(ICardService cardService, ICardNumberGenerator cardNumberGenerator)
        {
            _cardService = cardService ??
                           throw new ArgumentNullException(nameof(cardService));
            _cardNumberGenerator = cardNumberGenerator ?? throw new ArgumentNullException(nameof(cardNumberGenerator));
        }

        public User GenerateFakeUser() => new User("admin@admin.net");

        public IEnumerable<Card> GenerateFakeCards()
        {
            var date = DateTime.Today.AddYears(-2);
            // create fake cards
            var cards = new List<Card>
            {
                new Card
                {
                    CardNumber = _cardNumberGenerator.GenerateNewCardNumber(CardType.MAESTRO),
                    CardName = "my salary",
                    Currency = Currency.RUR,
                    CardType = CardType.MAESTRO,
                    DtOpenCard = date
                },
                new Card
                {
                    CardNumber = _cardNumberGenerator.GenerateNewCardNumber(CardType.VISA),
                    CardName = "my debt",
                    Currency = Currency.EUR,
                    CardType = CardType.VISA,
                    DtOpenCard = date
                },
                new Card
                {
                    CardNumber = _cardNumberGenerator.GenerateNewCardNumber(CardType.MASTERCARD),
                    CardName = "for my lovely wife",
                    Currency = Currency.USD,
                    CardType = CardType.MASTERCARD,
                    DtOpenCard = date
                }
            };
            cards.ForEach(card => _cardService.TryAddBonusOnOpen(card));

            return cards;
        }
    }
}