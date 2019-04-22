using System;
using Server.Exceptions;
using Server.Infrastructure;
using System.Linq;
using Server.Models;
using Server.Services.Checkers;

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
        public string GenerateNewCardNumber(CardType cardType)
		{
			var alfaBinsUseful = Enumerable.Empty<string>();
			switch (cardType)
			{
				case CardType.MIR:
					alfaBinsUseful = Constants.AlfaBins
						.Where(x => x.Substring(0, 1) == "2");
					break;
				case CardType.VISA:
					alfaBinsUseful = Constants.AlfaBins
						.Where(x => x.Substring(0, 1) == "4");
					break;
				case CardType.MAESTRO:
					alfaBinsUseful = Constants.AlfaBins
						.Where(x => x.Substring(0, 1) == "6" 
							|| new string[] { "50", "56", "57", "58", "59" }.Contains(x.Substring(0,2)));
					break;
				case CardType.MASTERCARD:
					alfaBinsUseful = Constants.AlfaBins
						.Where(x => new string[] { "51", "52", "53", "54", "55" }.Contains(x.Substring(0, 2)));
					break;
				default:
					return null;
			}
			if (!alfaBinsUseful.Any())
			{
				return null;
			}
				
			var random = new Random();
			var generateBin = alfaBinsUseful.ElementAt(random.Next(0, alfaBinsUseful.Count()-1));
			return generateBin + new string(char.Parse(random.Next(0,9).ToString()), 10);
		}

        /// <inheritdoc />
        /// <summary>
        /// Add bonus to new card when its opening
        /// </summary>
        /// <param name="card">Card to</param>
        /// <returns>Return <see langword="True"/>if operation is successfully</returns>
        public bool TryAddBonusOnOpen(Card card)
		{
			if(card == null)
			{
				return false;
			}
			var moneyOnCard = card.Money;
			card.Money += 10;
			if (moneyOnCard != card.Money)
			{
				card.Transaction.Add(new Transaction { Money = 10, TransactionTime = DateTime.Now });
				return true;
			}

			return false;
		}

		/// <inheritdoc />
		/// <summary>
		/// Get balance of the card
		/// </summary>
		/// <param name="card">Card to calculating</param>
		/// <returns><see langword="decimal" /> sum</returns>
		public decimal GetBalanceOfCard(Card card) 
			=> (card != null)? card.Money : 0;

        #endregion
    }
}