using Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services.Converters
{
    public class CurrencyConverter : ICurrencyConverter
    {

        /// <summary>
        /// Convert currency from to
        /// </summary>
        /// <param name="sum">Sum for convert</param>
        /// <param name="from">Convert from this currency</param>
        /// <param name="to">Convert to this currency</param>
        /// <returns>Converted sum on <see langword="decimal"/></returns>
        public decimal GetConvertSum(decimal sum, Currency from, Currency to)
        {

            decimal result = 0m;
            try
            {
                if (from == Currency.RUR && to == Currency.USD)
                    result = sum / 62.68m;
                else if (from == Currency.EUR && to == Currency.RUR)
                    result = sum * 72.64m;
                else
                    result = sum;
                return result;
            }
            catch (InvalidCastException e)
            {
                return -1m;
            }
        }
    }
}
