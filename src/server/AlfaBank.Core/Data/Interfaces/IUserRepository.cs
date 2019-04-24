using AlfaBank.Core.Models;

namespace AlfaBank.Core.Data.Interfaces
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