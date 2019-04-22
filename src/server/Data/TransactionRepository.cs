using System;
using System.Collections.Generic;
using System.Linq;
using Server.Data.Interfaces;
using Server.Models;

namespace Server.Data
{
    /// <inheritdoc />
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ICardRepository _cardRepository;

        /// <inheritdoc />
        public TransactionRepository(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository ?? throw new ArgumentNullException(nameof(cardRepository));
        }

        /// <inheritdoc />
        public IEnumerable<Transaction> GetTransactions(User user, string cardNumber, int skip, int take)
        {
            var card = _cardRepository.GetCard(user, cardNumber);

            if (card == null) return Enumerable.Empty<Transaction>();

            var transactions = card.Transactions.Skip(skip).Take(take);

            return transactions;
        }
    }
}