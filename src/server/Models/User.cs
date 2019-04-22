using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mail;
using Server.Exceptions;

namespace Server.Models
{
    /// <summary>
    /// User domain model
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class User
    {
        private MailAddress _mail;

        /// <summary>
        /// Create new User
        /// </summary>
        /// <param name="userName">Login of the user</param>
        public User(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new CriticalException("username is null or empty");

            UserName = userName;
        }

        /// <summary>
        /// Getter and setter username of the user for login
        /// </summary>
        public string UserName
        {
            get => _mail.ToString();

            private set
            {
                try
                {
                    _mail = new MailAddress(value);
                }
                catch (FormatException)
                {
                    throw new CriticalException("Email is invalid");
                }
            }
        }

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
        /// /// Getter and setter Birthday of the user
        /// </summary>
        /// <returns>Datetime</returns>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// Getter user card list
        /// </summary>
        public List<Card> Cards { get; } = new List<Card>();
    }
}