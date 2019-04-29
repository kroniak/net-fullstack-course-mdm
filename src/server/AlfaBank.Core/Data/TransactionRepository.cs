using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AlfaBank.Core.Data.Interfaces;
using AlfaBank.Core.Models;

namespace AlfaBank.Core.Data
{
    /// <inheritdoc />
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ICardRepository _cardRepository;

        [ExcludeFromCodeCoverage]
        public TransactionRepository(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository ?? throw new ArgumentNullException(nameof(cardRepository));
        }

        /// <inheritdoc />
        public IEnumerable<Transaction> Get(User user, string cardNumber, int skip, int take)
        {
            var card = _cardRepository.Get(user, cardNumber);

            if (card == null) return Enumerable.Empty<Transaction>();

            var transactions = card.Transactions.Skip(skip).Take(take);

            return transactions;
        }

        /// <inheritdoc />
        /// <summary>
        /// Get count transaction at last hour by user
        /// </summary>
        /// <param name="user">Current logged user</param>
        /// <returns>Transaction count</returns>
        public int CountLastHour(User user)
        {
            var startDateTime = DateTime.Now.AddHours(-1);
            var endDateTime = DateTime.Now;

            return Count(user, t => t.DateTime >= startDateTime && t.DateTime < endDateTime);
        }

        /// <inheritdoc />
        /// <summary>
        /// Get count transaction by user by filter
        /// </summary>
        /// <param name="user">Current logged user</param>
        /// <param name="filter">filter for transaction</param>
        /// <returns>Transaction count</returns>
        public int Count(User user, Func<Transaction, bool> filter)
        {
            var cards = _cardRepository.All(user);

            return cards.SelectMany(card => card.Transactions)
                .Where(filter).Count();
        }
    }
}