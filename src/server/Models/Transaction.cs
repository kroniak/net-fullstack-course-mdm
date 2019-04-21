using System.ComponentModel.DataAnnotations;
using Server.Infrastructure;

namespace Server.Models
{
    /// <summary>
    /// Transaction domain model
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Transaction user 
        /// </summary>
        public User User { get;set; }

        /// <summary>
        /// Transaction Card From
        /// </summary>
        public Card CardFrom { get;set; }

        /// <summary>
        /// Transaction Card To
        /// </summary>
        public Card CardTo { get;set; }

        /// <summary>
        /// Transaction Money
        /// </summary>
        [Required]
        [Range(0, 99999999.99)]
        public decimal Money { get;set; }

        /// <summary>
        /// Transaction Date
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public System.DateTime Date { get;set; }
    }
}