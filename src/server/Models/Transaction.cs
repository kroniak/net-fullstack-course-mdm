using System;

namespace Server.Models
{
    /// <summary>
    /// Transaction model
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Parent Card for transaction
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
        public bool IsCredit => CardToNumber == Card.CardNumber;
    }
}