using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AlfaBank.WebApi.Model
{
    public class TransactionDb
    {
        //key EF
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int CardDbId { get; set; } // внешний ключ
        public CardDb Cards { get; set; } // навигационное свойство

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
    }
}
