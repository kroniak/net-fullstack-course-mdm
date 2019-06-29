using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using AlfaBank.Core.Exceptions;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace AlfaBank.Core.Models
{
    /// <summary>
    /// User domain model
    /// </summary>
    public class User
    {
        /// <summary>
        /// Create new User
        /// </summary>
        /// <param name="userName">Login of the user</param>
        /// <param name="password">Password for the user</param>
        public User(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new CriticalException("username is null or empty");

            if (string.IsNullOrWhiteSpace(password))
                throw new CriticalException("password is null or empty");

            try
            {
                var mail = new MailAddress(userName);
                UserName = mail.ToString();
                Password = password;
            }
            catch (FormatException)
            {
                throw new CriticalException("Email is invalid");
            }
        }

        /// <summary>
        /// Identification
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Getter and setter username of the user for login
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// Hashed password
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// Getter and setter Surname of the user
        /// </summary>
        /// <returns><see langword="string"/></returns>
        public string Surname { get; set; }

        /// <summary>
        /// Getter and setter Firstname of the user
        /// </summary>
        /// <returns><see langword="string"/></returns>
        public string Firstname { get; set; }

        /// <summary>
        /// Getter and setter Birthday of the user
        /// </summary>
        /// <returns>Datetime</returns>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// Getter user card list
        /// </summary>
        public List<Card> Cards { get; set; }
    }
}