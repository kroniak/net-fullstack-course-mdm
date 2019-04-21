using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models
{
    /// <summary>
    /// Card transaction model
    /// </summary>
    public class Transaction
    {
        //key EF
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string CardNumber_From { get; set; }
        [Required]
        [MaxLength(100)]
        public string CardNumber_To { get; set; }
        [Required]
        public decimal Card_credit { get; set; }
        [Required]
        public decimal Card_Debit { get; set; }
        [Required]
        public DateTime EntryDate { get; set; }
        [Required]
        public int Card_credit_val { get; set; }
        [Required]
        public int Card_debit_val { get; set; }

    }
}
