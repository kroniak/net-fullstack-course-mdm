using AlfaBank.Core.Models;

namespace AlfaBank.Core.Data.Interfaces
{
    /// <inheritdoc />
    /// <summary>
    /// Repository for current user
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// Get current logged user from db
        /// </summary>
        /// <param name="userName">Username of the user</param>
        /// <param name="noTracking"></param>
        User GetCurrentUser(string userName, bool noTracking = true);

        /// <summary>
        /// Get current logged user from db with cards projection
        /// </summary>
        /// <param name="userName">Username of the user</param>
        User GetCurrentUserWithCards(string userName);
    }
}