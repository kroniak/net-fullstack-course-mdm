using AlfaBank.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AlfaBank.WebApi.Model
{
    public class CardDb
    {
      
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string CardNumber { get; set; }

        /// <summary>
        /// Short name of the cards
        /// </summary>
        /// <returns><see langword="string"/> short card name representation</returns>
        [Required]
        public string CardName { get; set; }

        /// <summary>
        /// Card <see cref="Currency"/>
        /// </summary>
        //public Currency Currency { get; set; }

        /// <summary>
        /// Card <see cref="CardType"/>
        /// </summary>
        /// 
        [Required]
        public CardType CardType { get; set; }

        /// <summary>
        /// DtOpenCard
        /// </summary>
        [Required]
        public DateTime DtOpenCard { get; set; } = DateTime.Now;

        /// <summary>
        /// Count year's
        /// </summary>
        public int ValidityYear { get; } = 3;
        [Required]
        public decimal Balance { get; set; }

        public int UserDbId { get; set; } // внешний ключ
        public UserDb Users { get; set; }  // навигационное свойство
    }
}
