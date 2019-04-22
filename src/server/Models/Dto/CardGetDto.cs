using System.ComponentModel.DataAnnotations;

namespace Server.Models.Dto
{
    /// <inheritdoc />
    /// <summary>
    /// Card model Dto to return from controller
    /// </summary>
    public class CardGetDto : CardPostDto
    {
        /// <summary>
        /// Card number
        /// </summary>
        /// <returns><see langword="string"/> card number representation</returns>
        [Required]
        [CreditCard]
        public string Number { get; set; }

        /// <summary>
        /// Balance of the card 
        /// </summary>
        [Required]
        public decimal Balance { get; set; }

        /// <summary>
        /// Date when card is expired
        /// </summary>
        [Required]
        [MinLength(4)]
        [MaxLength(5)]
        public string Exp { get; set; }
    }
}