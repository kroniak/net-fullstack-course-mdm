using System.Collections.Generic;
using Server.Models;

namespace Server.Data
{
    /// <summary>
    /// Repository for getting and setting card from storage
    /// </summary>
    public interface ICardRepository
    {
        /// <summary>
        /// Getter for cards
        /// </summary>
        IEnumerable<Card> GetCards();

        /// <summary>
        /// Get one card by number
        /// </summary>
        /// <param name="cardNumber">number of the cards</param>
        Card GetCard(string cardNumber);
    }
}