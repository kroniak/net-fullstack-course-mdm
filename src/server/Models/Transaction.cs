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
        public double Money { get;set; }

        /// <summary>
        /// Transaction Date
        /// </summary>
        public string Date { get;set; }
    }
}