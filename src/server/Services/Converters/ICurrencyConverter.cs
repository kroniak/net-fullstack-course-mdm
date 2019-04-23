using Server.Infrastructure;

namespace Server.Services.Converters
{
    /// <summary>
    /// Service for currency conversion
    /// </summary>
    public interface ICurrencyConverter
    {
        /// <summary>
        /// Convert currency from to
        /// </summary>
        /// <param name="sum">Sum for convert</param>
        /// <param name="from">Convert from this currency</param>
        /// <param name="to">Convert to this currency</param>
        /// <returns>Converted sum on <see langword="decimal"/></returns>
        decimal GetConvertedSum(decimal sum, Currency from, Currency to);
    }
}