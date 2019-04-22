using Server.Models;

namespace Server.Data.Interface
{
    /// <summary>
    /// Bank repository for current user
    /// </summary>
    public interface IUserRepository
    {
		/// <summary>
		/// Get current logged user
		/// </summary>
		/// <param name="userName">User's name</param>
		User GetCurrentUser(string UserName);
    }
}