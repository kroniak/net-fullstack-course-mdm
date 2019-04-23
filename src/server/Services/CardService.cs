using System;
using Server.Infrastructure;
using Server.Models;
using Server.Services.Checkers;
using Server.Services.Converters;
using Server.Services.Interfaces;

namespace Server.Services
{
    /// <inheritdoc />
    public class CardService : ICardService
    {
        private readonly ICardChecker _cardChecker;
        private readonly ICurrencyConverter _currencyConverter;

        public CardService(ICardChecker cardChecker, ICurrencyConverter currencyConverter)
        {
            _cardChecker = cardChecker ??
                           throw new ArgumentNullException(nameof(cardChecker));
            _currencyConverter = currencyConverter ??
                                 throw new ArgumentNullException(nameof(currencyConverter));
        }

        /// <inheritdoc />
        public CardType GetCardType(string number)
        {
            if (!_cardChecker.CheckCardNumber(number)) return CardType.OTHER;

            var firstDigit = number[0];
            var secondDigit = number[1];

            switch (firstDigit)
            {
                case '2':
                    return CardType.MIR;
                case '4':
                    return CardType.VISA;
                case '5'
                    when secondDigit == '0' || secondDigit > '5':
                    return CardType.MAESTRO;
                case '5'
                    when secondDigit >= '1' && secondDigit <= '5':
                    return CardType.MASTERCARD;
                case '6':
                    return CardType.MAESTRO;
                default:
                    return CardType.OTHER;
            }
        }

        /// <inheritdoc />
        public bool TryAddBonusOnOpen(Card card)
        {
            try
            {
                card.Transactions.Add(new Transaction
                {
                    Card = card,
                    CardToNumber = card.CardNumber,
                    Sum = _currencyConverter.GetConvertedSum(10M, Currency.RUR, card.Currency)
                });
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}