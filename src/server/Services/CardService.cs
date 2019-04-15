using System;
using Server.Infrastructure;
using Server.Services.Checkers;

namespace Server.Services
{
    /// <inheritdoc />
    /// <summary>
    /// Our implementing of the <see cref="T:Server.Services.ICardService" /> interface
    /// </summary>
    public class CardService : ICardService
    {
        private readonly ICardChecker _cardChecker;

        public CardService(ICardChecker cardChecker)
        {
            _cardChecker = cardChecker ??
                           throw new ArgumentException(nameof(cardChecker));
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

        #endregion
    }
}