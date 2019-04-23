using Models.Infrastructure;

namespace Business.Services.Converters
{
    /// <inheritdoc />
    public class CurrencyConverter : ICurrencyConverter
    {
        /// <inheritdoc />
        public decimal GetConvertedSum(decimal sum, Currency from, Currency to)
        {
            if (from == to) return sum;

            return sum * Constants.Currencies[from] / Constants.Currencies[to];
        }
    }
}