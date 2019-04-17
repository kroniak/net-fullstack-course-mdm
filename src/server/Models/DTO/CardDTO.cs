using Newtonsoft.Json;
using Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models.DTO
{
	/// <summary>
	/// Card DTO model.
	/// </summary>
	public class CardDTO
	{
		/// <summary>
		/// Card number.
		/// </summary>
		[Required]
		[MaxLength(16, ErrorMessage = "Card's number more than 16 digits")]
		[MinLength(16, ErrorMessage = "Card's number less than 16 digits")]
		[RegularExpression("+[0-9]", ErrorMessage = "Used invalid characters")]
		public string CardNumber { get; set; }

		/// <summary>
		/// Short name of the cards.
		/// </summary>
		public string CardName { get; set; }

		/// <summary>
		/// Money on the card.
		/// </summary>		
		[Range(0, 1_000_000_000, ErrorMessage = "Money have invalid value")]
		public decimal Money { get; set; }

		/// <summary>
		/// Currency in which money.
		/// </summary>
		public Currency Currency { get; set; }

		/// <summary>
		/// Card expiry date.
		/// </summary>
		public DateTime Validity { get; set; }
	}
}
