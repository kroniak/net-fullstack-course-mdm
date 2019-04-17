using System;
using System.Collections.Generic;

namespace Server.Models
{
    public class UserDto
    {
        /// <summary>
        /// Getter and setter username of the user for login
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// Getter user card list
        /// </summary>
        public List<CardDto> Cards { get; set;}
    }
}