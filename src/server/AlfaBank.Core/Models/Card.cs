using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AlfaBank.Core.Infrastructure;

namespace AlfaBank.Core.Models
{
    /// <summary>
    /// Card model
    /// </summary>
    public class Card
    {
        /// <summary>
        /// Identification
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// User FK object
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Card number
        /// </summary>
        /// <returns><see langword="string"/> card number representation</returns>
        [MinLength(12)]
        [MaxLength(19)]
        [Required]
        public string CardNumber { get; set; }

        /// <summary>
        /// Short name of the cards
        /// </summary>
        /// <returns><see langword="string"/> short card name representation</returns>
        [MinLength(3)]
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
        /// Get balance of the card. Dont save it in db
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
        /// Get balance of the card round to 2. Dont save it in db
        /// </summary>
        public decimal RoundBalance => Math.Round(Balance, 2, MidpointRounding.ToEven);
    }
}