using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    /// <summary>
    /// User domain model
    /// </summary>
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Getter and setter username of the user for login
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string UserName { get;  set; }
        [Required]
        [MaxLength(100)]
        public string UserPasport { get; set; }

        /// <summary>
        /// Getter user card list
        /// </summary>


        // TODO add fields
    }
}