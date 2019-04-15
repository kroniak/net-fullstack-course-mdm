using Server.Infrastructure;

namespace Server.Services.Converters
{
    public interface ICurrencyConverter
    {
        /// <summary>
        /// Convert currency from to
        /// </summary>
        /// <param name="sum">Sum for convert</param>
        /// <param name="from">Convert from this currency</param>
        /// <param name="to">Convert to this currency</param>
        /// <returns>Converted sum on <see langword="decimal"/></returns>
        decimal GetConvertSum(decimal sum, Currency from, Currency to);
    }
}