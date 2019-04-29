using System;
using System.Collections.Generic;
using AlfaBank.Core.Models;

namespace AlfaBank.Core.Data.Interfaces
{
    /// <summary>
    /// Repository for getting and setting transactions from storage
    /// </summary>
    public interface ITransactionRepository
    {
        /// <summary>
        /// Get range of transactions
        /// </summary>
        /// <param name="user"> current user</param>
        /// <param name="cardNumber">card number</param>
        /// <param name="from">how much to skip</param>
        /// <param name="to">how much to take</param>
        IEnumerable<Transaction> Get(User user, string cardNumber, int from, int to);

        /// <summary>
        /// Get count transaction at last hour by user
        /// </summary>
        /// <param name="user">Current logged user</param>
        /// <returns>Transaction count</returns>
        int CountLastHour(User user);

        /// <summary>
        /// Get count transaction by user by filter
        /// </summary>
        /// <param name="user">Current logged user</param>
        /// <param name="filter">filter for transaction</param>
        /// <returns>Transaction count</returns>
        int Count(User user, Func<Transaction, bool> filter);
    }
}