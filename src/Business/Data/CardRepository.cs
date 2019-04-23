using System.Collections.Generic;
using System.Linq;
using Business.Data.Interfaces;
using Models;
using Business.Extensions;

namespace Business.Data
{
    /// <inheritdoc />
    public class CardRepository : ICardRepository
    {
        /// <inheritdoc />
        public Card GetCard(User user, string cardNumber) =>
            GetCards(user)
                .FirstOrDefault(c => c.CardNumber == cardNumber.ToNormalizedCardNumber());

		/// <inheritdoc />
		public List<Card> GetCards(User user) => user.Cards;
	}
}