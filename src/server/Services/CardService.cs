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

        public CardService(ICardChecker cardChecker)
        {
            _cardChecker = cardChecker ??
                           throw new CriticalException(nameof(cardChecker));
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
        public bool TryAddBonusOnOpen(Card card)
        {
            if (card == null) return false;
            
            if(card.Currency == Currency.RUR) 
            {
                card.Transactions.Add(new Transaction() 
                {
                    Date = DateTime.Now,
                    Money = 10M,
                    CardFrom = card,
                    CardTo = card
                });
            }
            else 
            {
                card.Transactions.Add(new Transaction() 
                {
                    Date = DateTime.Now,
                    Money = new CurrencyConverter().GetConvertSum(10, Currency.RUR, card.Currency),
                    CardFrom = card,
                    CardTo = card
                });
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
            
            var balance = 0M;
            
            foreach(var transaction in card.Transactions)
            {
                balance += transaction.Money;
            }
            return balance;
        }

        #endregion
    }
}