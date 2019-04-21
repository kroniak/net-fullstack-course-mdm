using Server.Infrastructure;

namespace Server.Models
{
    /// <summary>
    /// DTO Card model
    /// </summary>
    public class CardDto
    {
        /// <summary>
        /// Card Number
        /// </summary>
        public string CardNumber {get; set;}
        
        /// <summary>
        /// Card type
        /// </summary>
        public CardType CardType { get; set; }
        
        /// <summary>
        /// Currency of Card
        /// </summary>
        public Currency Currency { get; set; }
    }
}