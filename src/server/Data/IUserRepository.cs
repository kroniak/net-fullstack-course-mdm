using Server.Models;

namespace Server.Data
{
    /// <summary>
    /// Bank repository for current user
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Get current logged user
        /// </summary>
        User GetCurrentUser();
    }
}