using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models
{
	public class Transaction
	{
		/// <summary>
		///Transaction time.
		/// </summary>
		public DateTime TransactionTime { get; set; }

		/// <summary>
		///User from which the money came.
		/// </summary>
		/// <returns>Card's number</returns>
		public string CardNumberFrom { get; set; }

		/// <summary>
		///User who got the money.
		/// </summary>
		/// <returns>Card's number</returns>
		public string CardNumberTo { get; set; }

		private decimal _money;

		/// <summary>
		///Money in transaction.
		/// </summary>
		public decimal Money
		{
			get
			{
				return _money;
			}
			set
			{
				if(value > 0)
				{
					_money = value;
				}
			}
		}
	}
}
