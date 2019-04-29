using System.Collections.Generic;
using AlfaBank.Core.Models;

namespace AlfaBank.Core.Data.Interfaces
{
    /// <inheritdoc />
    /// <summary>
    /// Repository for getting and setting card from storage
    /// </summary>
    public interface ICardRepository : IRepository<Card>
    {
        /// <summary>
        /// Get one card by number
        /// </summary>
        /// <param name="user"> current user</param>
        /// <param name="cardNumber">number of the cards</param>
        Card Get(User user, string cardNumber);

        /// <summary>
        /// Get all cards of the user with transactions
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Cards Enumerable</returns>
        IEnumerable<Card> GetAllWithTransactions(User user);

        /// <summary>
        /// Get one card by number with transactions
        /// </summary>
        /// <param name="user"> current user</param>
        /// <param name="cardNumber">number of the cards</param>
        /// <param name="noTracking">flag is noTracking</param>
        Card GetWithTransactions(User user, string cardNumber, bool noTracking = false);

        /// <summary>
        /// Get cards count for the current user
        /// </summary>
        /// <param name="user"> current user</param>
        /// <returns>Count of the cards</returns>
        int Count(User user);
    }
}