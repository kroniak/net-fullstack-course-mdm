using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AlfaBank.Core.Data.Interfaces;
using AlfaBank.Core.Models;

namespace AlfaBank.Core.Data.Repositories
{
    /// <inheritdoc cref="IUserRepository" />
    /// <summary>
    /// Realisation User Repository for EF
    /// </summary>
    public class UserRepository : Repository<User>, IUserRepository
    {
        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        public UserRepository(SqlContext context) : base(context)
        {
        }

        /// <inheritdoc />
        public User GetCurrentUser(string userName, bool noTracking = true) =>
            Get(u => u.UserName == userName, noTracking)
                .FirstOrDefault();
    }
}