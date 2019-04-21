using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateClose { get; set; }

        /// <summary>
        /// Card type
        /// </summary>
        public CardType CardType { get; set; }

        /// <summary>
        /// Currency of Card
        /// </summary>
        public Currency Currency { get; set; }

        /// <summary>
        /// list Transaction
        /// </summary>
        public List<Transaction> Transactions { get; } = new List<Transaction>();
    }
}