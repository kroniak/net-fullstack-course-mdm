namespace AlfaBank.WebApi.Services
{
    /// <summary>
    /// Simple realization of the basic auth service
    /// </summary>
    public interface ISimpleAuthenticateService
    {
        /// <summary>
        /// Check the user credentials and return the token result
        /// </summary>
        /// <param name="userName">Username</param>
        /// <param name="password">Password not hashed</param>
        /// <returns>JWT Token if credential is valid</returns>
        string CheckUserCredentials(string userName, string password);
    }
}