using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AlfaBank.WebApi.Model
{
    public class UserDb
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Getter and setter username of the user for login
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string UserName { get; set; }
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
    }
}
