using Server.Infrastructure;

namespace Server.Services.Converters
{
    public class CurrencyConverter
    {
        /// <inheritdoc/>
        decimal GetConvertSum(decimal sum, Currency from, Currency to)
        {
            if(sum <= 0) return 0M;
            if(from == to) return sum;

            if ((from == Currency.USD || from == Currency.EUR) && (to == Currency.RUR))
            {
                return sum * Constants.Currencies[from];
            } 

            return sum / Constants.Currencies[to];
        }
    }
}