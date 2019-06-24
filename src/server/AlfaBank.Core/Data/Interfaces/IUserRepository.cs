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
        /// <param name="noTracking">Turn on noTracking or not</param>
        /// <returns>User</returns>
        User GetUser(string userName, bool noTracking = true);

        /// <summary>
        /// Get user with password with noTracking
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>User</returns>
        User GetSecureUser(string userName);
    }
}