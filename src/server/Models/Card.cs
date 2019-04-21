using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    /// <summary>
    /// Card domain model
    /// </summary>
    public class Card
    {
        /// <summary>
        /// Card 
        /// </summary>
        /// <returns>string card  DB</returns>

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string CardNumber { get; set; }

        /// <summary>
        /// Short name of the cards
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string CardName { get; set; }
        /// <summary>
        /// Valid date of the cards
        /// </summary>
        public DateTime CardValidDate { get; set; }

        public int UserId { get; set; } // внешний ключ
        public User Users { get; set; }  // навигационное свойство

        // TODO add fields
    }

}