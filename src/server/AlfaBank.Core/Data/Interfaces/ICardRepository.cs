using System.Collections.Generic;
using AlfaBank.Core.Models;

namespace AlfaBank.Core.Data.Interfaces
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
        IEnumerable<Card> All(User user);

        /// <summary>
        /// Get one card by number
        /// </summary>
        /// <param name="user"> current user</param>
        /// <param name="cardNumber">number of the cards</param>
        Card Get(User user, string cardNumber);

        /// <summary>
        /// Add card to user list of cards 
        /// </summary>
        /// <param name="user"> current user</param>
        /// <param name="card"> card to add</param>
        void Add(User user, Card card);

        /// <summary>
        /// Remove card from list
        /// </summary>
        /// <param name="user"> current user</param>
        /// <param name="card"> card to remove</param>
        void Remove(User user, Card card);

        /// <summary>
        /// Get cards count for the current user
        /// </summary>
        /// <param name="user"> current user</param>
        /// <returns>Count of the cards</returns>
        int Count(User user);
    }
}