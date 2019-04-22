using System.Collections.Generic;
using Server.Models;

namespace Server.Data.Interfaces
{
    /// <summary>
    /// Repository for getting and setting card from storage
    /// </summary>
    public interface ICardRepository
    {
        /// <summary>
        /// Getter for cards
        /// <param name="user"> current user</param>
        /// </summary>
        List<Card> GetCards(User user);

        /// <summary>
        /// Get one card by number
        /// </summary>
        /// <param name="user"> current user</param>
        /// <param name="cardNumber">number of the cards</param>
        Card GetCard(User user, string cardNumber);
    }
}