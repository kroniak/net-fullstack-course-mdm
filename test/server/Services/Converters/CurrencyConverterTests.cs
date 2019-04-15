using System;
using Server.Infrastructure;
using Server.Services.Converters;
using Xunit;

namespace ServerTest.Services.Converters
{
    public class CurrencyConverterTests
    {
        private readonly ICurrencyConverter _currencyConverter;

        public CurrencyConverterTests(ICurrencyConverter currencyConverter)
        {
            _currencyConverter = currencyConverter;
        }

        [Theory]
        [InlineData(Currency.RUR, Currency.USD, 1000, "15.954052329291640076579451181")]
        [InlineData(Currency.EUR, Currency.RUR, 100, "7264")]
        [InlineData(Currency.RUR, Currency.RUR, 100, "100")]
        public void GetConvertSum_Passed(Currency from, Currency to, decimal valueIn, string valueOut)
        {
            // Arrange
            var validConvertedSum = Convert.ToDecimal(valueOut);
            // Act
            var convertedSum = _currencyConverter.GetConvertSum(valueIn, from, to);
            // Assert
            Assert.Equal(validConvertedSum, convertedSum);
        }
    }
}