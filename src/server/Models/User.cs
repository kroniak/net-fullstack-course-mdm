using System;
using System.Collections.Generic;

namespace Server.Models
{
    /// <summary>
    /// User domain model
    /// </summary>
    public class User
    {
        public User(string userName)
        {
            // TODO return own Exception class
            if (string.IsNullOrWhiteSpace(userName))
                throw new Exception("username is null or empty");

            UserName = userName;
        }

		public User(string userName, Card card) : this(userName)
		{
			Cards.Add(card);
		}

        /// <summary>
        /// Getter and setter username of the user for login
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// Getter user card list
        /// </summary>
        public List<Card> Cards { get; } = new List<Card>();

        // TODO add fields
    }
}