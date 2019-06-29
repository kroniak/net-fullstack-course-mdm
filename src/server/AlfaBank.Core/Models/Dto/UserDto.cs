using System.ComponentModel.DataAnnotations;

namespace AlfaBank.Core.Models.Dto
{
    /// <summary>
    /// User dto for Login procedure
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Username for login
        /// </summary>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// Not hashed password for login
        /// </summary>
        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }
}