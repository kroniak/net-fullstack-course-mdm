using System;
using System.ComponentModel.DataAnnotations;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Server.Models.Dto
{
    /// <inheritdoc />
    /// <summary>
    /// Transaction DTO model
    /// </summary>
    public class TransactionGetDto : TransactionPostDto
    {
        /// <summary>
        /// Public Time of transaction
        /// </summary>
        [Required]
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Flag that operation is credit - else debit
        /// </summary>
        [Required]
        public bool IsCredit { get; set; }

        /// <summary>
        /// From card number
        /// </summary>
        public new string From { get; set; }

        /// <summary>
        /// To card number
        /// </summary>
        [Required]
        public new string To { get; set; }
    }
}