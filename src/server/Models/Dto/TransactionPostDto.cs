using System.ComponentModel.DataAnnotations;

namespace Server.Models.Dto
{
    /// <summary>
    /// Transaction DTO model
    /// </summary>
    public class TransactionPostDto
    {
        /// <summary>
        /// Sum in transaction
        /// </summary>
        /// <returns><see langword="decimal"/>representation of the sum transaction</returns>
        [Required]
        [Range(0.0, 1000000.0)]
        public decimal Sum { get; set; }

        /// <summary>
        /// LFrom card number
        /// </summary>
        [Required]
        [CreditCard]
        public string From { get; set; }

        /// <summary>
        /// To card number
        /// </summary>
        [Required]
        [CreditCard]
        public string To { get; set; }
    }
}