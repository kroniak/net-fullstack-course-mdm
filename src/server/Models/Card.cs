using System;
using System.Collections.Generic;
using System.Linq;
using Server.Infrastructure;

namespace Server.Models
{
    /// <summary>
    /// Card model
    /// </summary>
    public class Card
    {
        /// <summary>
        /// Card number
        /// </summary>
        /// <returns><see langword="string"/> card number representation</returns>
        public string CardNumber { get; set; }

        /// <summary>
        /// Short name of the cards
        /// </summary>
        /// <returns><see langword="string"/> short card name representation</returns>
        public string CardName { get; set; }

        /// <summary>
        /// Card <see cref="Currency"/>
        /// </summary>
        public Currency Currency { get; set; }

        /// <summary>
        /// Card <see cref="CardType"/>
        /// </summary>
        public CardType CardType { get; set; }

        /// <summary>
        /// DtOpenCard
        /// </summary>
        public DateTime DtOpenCard { get; set; } = DateTime.Now;

        /// <summary>
        /// Count year's
        /// </summary>
        public int ValidityYear { get; } = 3;

        /// <summary>
        /// Return all transaction of the card
        /// </summary>
        public List<Transaction> Transactions { get; } = new List<Transaction>();

        /// <summary>
        /// Get balance of the card
        /// </summary>
        public decimal Balance
        {
            get
            {
                var credit = Transactions.Where(x => x.CardToNumber == CardNumber).Sum(x => x.Sum);
                var debit = Transactions.Where(x => x.CardFromNumber == CardNumber).Sum(x => x.Sum);
                return credit - debit;
            }
        }

        /// <summary>
        /// Get balance of the card round to 2
        /// </summary>
        public decimal RoundBalance => Math.Round(Balance, 2, MidpointRounding.ToEven);
    }
}