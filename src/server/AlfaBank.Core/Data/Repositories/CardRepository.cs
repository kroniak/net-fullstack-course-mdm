using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AlfaBank.Core.Data.Interfaces;
using AlfaBank.Core.Extensions;
using AlfaBank.Core.Models;
using Microsoft.Extensions.Caching.Memory;

namespace AlfaBank.Core.Data.Repositories
{
    /// <inheritdoc cref="ICardRepository" />
    public class CardRepository : Repository<Card>, ICardRepository
    {
        private readonly IMemoryCache _memoryCache;

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        public CardRepository(
            SqlContext context,
            IMemoryCache memoryCache) : base(context)
        {
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        private T GetOrSetFromCache<T>(string key, User user, Func<User, T> action)
        {
            // Look for cache key.
            if (!_memoryCache.TryGetValue(key + "_" + user.UserName, out T cacheEntry))
            {
                // Key not in cache, so get data.
                cacheEntry = action(user);

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromSeconds(10));

                // Save data in cache.
                _memoryCache.Set(key + "_" + user.UserName, cacheEntry, cacheEntryOptions);
            }

            return cacheEntry;
        }

        private T GetOrSetFromCache<T>(string key, User user, string cardNumber, Func<User, string, T> action)
        {
            // Look for cache key.
            if (!_memoryCache.TryGetValue(key + "_" + user.UserName, out T cacheEntry))
            {
                // Key not in cache, so get data.
                cacheEntry = action(user, cardNumber);

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromSeconds(10));

                // Save data in cache.
                _memoryCache.Set(key + "_" + user.UserName, cacheEntry, cacheEntryOptions);
            }

            return cacheEntry;
        }

        /// <inheritdoc />
        public Card Get(User user, string cardNumber) =>
            GetOrSetFromCache("Get", user, cardNumber, (u, s) =>
                Get(c =>
                            c.User.Id == user.Id &&
                            c.CardNumber == cardNumber.ToNormalizedCardNumber(),
                        false)
                    .FirstOrDefault());

        /// <inheritdoc />
        public IEnumerable<Card> GetAllWithTransactions(User user) =>
            GetOrSetFromCache("GetAllWithTransactions", user, u => GetWithInclude(
                    c =>
                        c.User.Id == user.Id,
                    true,
                    c => c.Transactions)
                .ToArray());

        /// <inheritdoc />
        public Card GetWithTransactions(User user, string cardNumber, bool noTracking = false) =>
            GetOrSetFromCache("GetWithTransactions", user, cardNumber, (u, s) =>
                GetWithInclude(
                        c =>
                            c.User.Id == user.Id &&
                            c.CardNumber == cardNumber.ToNormalizedCardNumber(),
                        noTracking,
                        c => c.Transactions)
                    .FirstOrDefault());

        /// <inheritdoc />
        public int Count() => Get().Count();
    }
}