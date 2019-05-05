using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AlfaBank.Core.Data.Interfaces;
using AlfaBank.Core.Extensions;
using AlfaBank.Core.Models;

namespace AlfaBank.Core.Data.Repositories
{
    /// <inheritdoc cref="ICardRepository" />
    public class CardRepository : Repository<Card>, ICardRepository
    {
        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        public CardRepository(SqlContext context) : base(context)
        {
        }

        /// <inheritdoc />
        public Card Get(User user, string cardNumber) =>
            Get(c =>
                        c.User.Id == user.Id &&
                        c.CardNumber == cardNumber.ToNormalizedCardNumber(),
                    false)
                .FirstOrDefault();

        /// <inheritdoc />
        public IEnumerable<Card> GetAllWithTransactions(User user) =>
            GetWithInclude(
                    c =>
                        c.User.Id == user.Id,
                    true,
                    c => c.Transactions)
                .ToArray();

        /// <inheritdoc />
        public Card GetWithTransactions(User user, string cardNumber, bool noTracking = false) =>
            GetWithInclude(
                    c =>
                        c.User.Id == user.Id &&
                        c.CardNumber == cardNumber.ToNormalizedCardNumber(),
                    noTracking,
                    c => c.Transactions)
                .FirstOrDefault();

        /// <inheritdoc />
        public int Count(User user) => Get(c => c.User.Id == user.Id).Count();
    }
}