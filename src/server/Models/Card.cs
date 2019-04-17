using Server.Infrastructure;
using System;

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

		private decimal _money;

		/// <summary>
		/// Money on the card
		/// </summary>
		public decimal Money
		{
			set
			{
				if (value < 1_000_000_000)
				{
					_money = value;
				}
			}
			get { return _money; }
		}

		/// <summary>
		/// Currency in which money
		/// </summary>
		public Currency Currency { get; set; }


		private DateTime _validity;

		/// <summary>
		/// Card expiry date
		/// </summary>
		public DateTime Validity
		{
			set
			{
				if (value.Year > DateTime.Now.Year + 1)
				{
					_validity = value.Date;
				}
			}
			get
			{
				return _validity;
			}
		}
	}
}