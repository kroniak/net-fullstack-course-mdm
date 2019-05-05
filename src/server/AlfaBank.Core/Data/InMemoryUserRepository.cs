using System;
using System.Diagnostics.CodeAnalysis;
using AlfaBank.Core.Data.Interfaces;
using AlfaBank.Core.Models;

namespace AlfaBank.Core.Data
{
    /// <inheritdoc />
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly User _currentUser;

        [ExcludeFromCodeCoverage]
        public InMemoryUserRepository(User currentUser)
        {
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        /// <inheritdoc />
        public User GetCurrentUser() => _currentUser;
    }
}