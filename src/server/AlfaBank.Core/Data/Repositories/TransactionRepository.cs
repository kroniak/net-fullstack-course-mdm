using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using AlfaBank.Core.Data.Interfaces;
using AlfaBank.Core.Models;

namespace AlfaBank.Core.Data.Repositories
{
    /// <inheritdoc cref="ITransactionRepository" />
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        public TransactionRepository(SqlContext context) : base(context)
        {
        }

        /// <inheritdoc />
        public IEnumerable<Transaction> Get(User user, string cardNumber, int skip, int take) =>
            GetWithInclude(t =>
                        t.Card.CardNumber == cardNumber &&
                        t.Card.User.Id == user.Id,
                    true,
                    t => t.Card)
                .OrderBy(t => t.Id)
                .Skip(skip)
                .Take(take)
                .ToArray();

        /// <inheritdoc />
        /// <summary>
        /// Get count transaction at last hour by user
        /// </summary>
        /// <returns>Transaction count</returns>
        public int CountLastHour()
        {
            var startDateTime = DateTime.Now.AddHours(-1);
            var endDateTime = DateTime.Now;

            return Count(t => t.DateTime >= startDateTime && t.DateTime < endDateTime);
        }

        /// <inheritdoc />
        /// <summary>
        /// Get count transaction by user by filter
        /// </summary>
        /// <param name="filter">filter for transaction</param>
        /// <returns>Transaction count</returns>
        public int Count(Expression<Func<Transaction, bool>> filter) =>
            Get(filter).Count();
    }
}