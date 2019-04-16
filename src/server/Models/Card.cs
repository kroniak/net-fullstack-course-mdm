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

        // TODO add fields
    }
}