using System.ComponentModel.DataAnnotations;

namespace Server.Models {
    public class TransactionDto {
        /// <summary>
        /// Transaction user
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Transaction Card To
        /// </summary>
        /// 
        /// Обезличенная
        public Card CardTo { get; set; }

        /// <summary>
        /// Transaction Card From
        /// </summary>
        /// 
        /// Обезличенная
        public Card CardFrom { get; set; }

        /// <summary>
        /// Transaction Money
        /// </summary>
        [Required]
        [Range (0, 99999999.99)]
        public double Money { get; set; }

        /// <summary>
        /// Transaction Date
        /// </summary>
        public string Date { get; set; }
    }
}