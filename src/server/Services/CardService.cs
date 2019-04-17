using System;
using Server.Exceptions;
using Server.Infrastructure;
using Server.Models;
using Server.Services.Checkers;
using Server.Services.Converters;

namespace Server.Services
{
    /// <inheritdoc />
    public class CardService : ICardService
    {
        private readonly ICardChecker _cardChecker;
        private readonly ICurrencyConverter _currencyConverter;

        public CardService(ICardChecker cardChecker)
        {
            _cardChecker = cardChecker ??
                           throw new CriticalException(nameof(cardChecker));
        }

        public CardService(ICardChecker cardChecker, ICurrencyConverter currencyConverter)
        {
            _cardChecker = cardChecker ??
                           throw new CriticalException(nameof(cardChecker));

            _currencyConverter = currencyConverter ??
                            throw new CriticalException(nameof(currencyConverter));
        }

        #region ICardService

        /// <inheritdoc />
        /// <summary>
        /// Get card type by card number
        /// </summary>
        /// <param name="number">card number in any format</param>
        /// <returns>Return enum CardType</returns>
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
        public string GenerateNewCardNumber(CardType cardType) => throw new NotImplementedException();

        /// <inheritdoc />
        /// <summary>
        /// Add bonus to new card when its opening
        /// </summary>
        /// <param name="card">Card to</param>
        /// <returns>Return <see langword="True"/>if operation is successfully</returns>
        public bool TryAddBonusOnOpen(ref Card card)
        {
            if (card == null) return false;
            if (string.IsNullOrWhiteSpace(card.CardNumber)) return false;

            if (!_cardChecker.CheckCardNumber(card.CardNumber)) return false;
            
            if(card.Currency == Currency.RUR) card.Money += 10;
            else 
            {
                card.Money += _currencyConverter.GetConvertSum(10, Currency.RUR, card.Currency);
            }

            return true;
        }

        /// <inheritdoc />
        /// <summary>
        /// Get balance of the card
        /// </summary>
        /// <param name="card">Card to calculating</param>
        /// <returns><see langword="decimal" /> sum</returns>
        public decimal GetBalanceOfCard(Card card)
        {
            if (card == null) return 0;
            if (string.IsNullOrWhiteSpace(card.CardNumber)) return 0;
            // Необходима ли проверка на валидность, или уверены в корректности созданной карты
            // А проверка на дату закрытия?
            if (!_cardChecker.CheckCardNumber(card.CardNumber)) return 0;

            return card.Money;
        }

        #endregion
    }
}