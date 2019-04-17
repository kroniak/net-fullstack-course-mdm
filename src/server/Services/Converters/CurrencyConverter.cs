using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.Infrastructure;

namespace Server.Services.Converters
{
	public class CurrencyConverter : ICurrencyConverter
	{
		public decimal GetConvertSum(decimal sum, Currency from, Currency to)
		{
			if( sum <= 0 )
			{
				return 0;
			}

			return sum / Constants.Currencies[to] * Constants.Currencies[from]; 
		}
	}
}
