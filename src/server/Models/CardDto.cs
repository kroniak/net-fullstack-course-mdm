using Server.Infrastructure;

namespace Server.Models
{
    /// <summary>
    /// DTO Card model
    /// </summary>
    public class CardDto
    {
        private string _cardNumber;
        public string CardNumber { 
            get
            {
                // Придумать способ замены символов на звездочки с 5 по 12
                return _cardNumber;
            }
            set { _cardNumber = value; }
        }
        
        /// <summary>
        /// Short name of the cards
        /// </summary>
        public string CardName { get; set; }
        
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