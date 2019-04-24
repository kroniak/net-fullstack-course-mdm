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
    }
}