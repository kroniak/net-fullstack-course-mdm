using System.Collections.Generic;

namespace Server.Data
{
    /// <summary>
    /// Repository for getting and setting transactions from storage
    /// </summary>
    public interface ITransactionRepository
    {
        /// <summary>
        /// Get range of transactions
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="from">from range</param>
        /// <param name="to">to range</param>
        IEnumerable<object> GetTransactions(string cardNumber, int from, int to);
    }
}