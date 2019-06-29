using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace AlfaBank.Core.Models
{
    /// <summary>
    /// Transaction model
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Identification
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Card FK object
        /// </summary>
        [Required]
        public int CardId { get; set; }

        /// <summary>
        /// Card navigation property
        /// </summary>
        public Card Card { get; set; }

        /// <summary>
        /// Public Time of transaction
        /// </summary>
        public DateTime DateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// Sum in transaction
        /// </summary>
        /// <returns><see langword="decimal"/>representation of the sum transaction</returns>
        [Required]
        [Column(TypeName = "decimal(16, 2)")]
        public decimal Sum { get; set; }

        /// <summary>
        /// Link to valid card
        /// </summary>
        public string CardFromNumber { get; set; }

        /// <summary>
        /// Link to valid card
        /// </summary>
        public string CardToNumber { get; set; }

        /// <summary>
        /// Flag is transaction is credit. Dont save it in db
        /// </summary>
        [NotMapped]
        public bool IsCredit => CardToNumber == Card.CardNumber;
    }
}