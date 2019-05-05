using System.Collections.Generic;
using System.Linq;
using AlfaBank.Core.Data.Interfaces;
using AlfaBank.Core.Extensions;
using AlfaBank.Core.Models;

namespace AlfaBank.Core.Data
{
    /// <inheritdoc />
    public class CardRepository : ICardRepository
    {
        /// <inheritdoc />
        public Card Get(User user, string cardNumber) =>
            All(user)
                .FirstOrDefault(c => c.CardNumber == cardNumber.ToNormalizedCardNumber());

        /// <inheritdoc />
        public void Add(User user, Card card)
        {
            if (card == null) return;

            if (All(user).Any(c => c.CardNumber == card.CardNumber)) return;

            user.Cards.Add(card);
        }

        /// <inheritdoc />
        public void Remove(User user, Card card)
        {
            if (card == null) return;

            var deleteCard = Get(user, card.CardNumber);
            if (deleteCard == null) return;

            user.Cards.Remove(deleteCard);
        }

        /// <inheritdoc />
        public int Count(User user) => user.Cards.Count;

        /// <inheritdoc />
        public IEnumerable<Card> All(User user) => user.Cards;
    }
}