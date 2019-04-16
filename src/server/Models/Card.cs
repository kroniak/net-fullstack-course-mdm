using Server.Infrastructure;

namespace Server.Models
{
    /// <summary>
    /// Card domain model
    /// </summary>
    public class Card
    {
        /// <summary>
        /// Card number.
        /// </summary>
        /// <returns>string card number representation</returns>
        public string CardNumber { get; set; }

        /// <summary>
        /// Short name of the cards
        /// </summary>
        public string CardName { get; set; }

        /// <summary>
        /// Date close of the cards
        /// </summary>
        public string DateClose { get; set; }

        /// <summary>
        /// Card type
        /// </summary>
        public CardType CardType { get; set; }

        /// <summary>
        /// Currency of Card
        /// </summary>
        public Currency Currency { get; set; }

        /// <summary>
        /// Money of Card
        /// </summary>
        public double Money { get; set; }
    }
}