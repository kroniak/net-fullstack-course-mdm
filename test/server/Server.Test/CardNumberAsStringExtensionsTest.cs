using AlfaBank.Core.Extensions;
using Xunit;

namespace Server.Test
{
    public class CardNumberAsStringExtensionsTest
    {
        [Theory]
        [InlineData("123456781234ssss56d7S8")]
        [InlineData("  123456781234ssss56d7S8.")]
        public void ToNormalizedCardNumber_ValidString_ReturnValid(string s)
        {
            // Arrange
            const string expectedNumber = "1234567812345678";

            // Act
            var result = s.ToNormalizedCardNumber();

            // Assert
            Assert.Equal(expectedNumber, result);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void ToNormalizedCardNumber_InvalidString_ReturnNull(string s)
        {
            // Act
            var result = s.ToNormalizedCardNumber();

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData("123456781234ssss56d7S8")]
        [InlineData("  123456781234ssss56d7S8.")]
        public void CardNumberWatermark_ValidString_ReturnValid(string s)
        {
            // Arrange
            const string expectedNumber = "1234XXXXXXXX5678";

            // Act
            var result = s.CardNumberWatermark();

            // Assert
            Assert.Equal(expectedNumber, result);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void CardNumberWatermark_InvalidString_ReturnNull(string s)
        {
            // Act
            var result = s.CardNumberWatermark();

            // Assert
            Assert.Null(result);
        }
    }
}