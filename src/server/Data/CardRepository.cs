using System.Collections.Generic;
using System.Linq;
using Server.Data.Interfaces;
using Server.Models;
using Server.Services.Extensions;

namespace Server.Data
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