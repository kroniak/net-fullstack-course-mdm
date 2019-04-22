using Server.Models;

namespace Server.Data.Interfaces
{
    /// <summary>
    /// Repository for current user
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Get current logged user
        /// </summary>
        User GetCurrentUser();
    }
}