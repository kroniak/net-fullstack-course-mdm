using System;
using Server.Services.Checkers;
using Xunit;

namespace ServerTest.Services.Checkers
{
    /// <summary>
    /// Tests for <see cref="CardChecker"/>>
    /// </summary>
    public class CardCheckerTests
    {
        private readonly ICardChecker _cardChecker = new CardChecker();

        [Theory]
        [InlineData("4083967629457310")]
        [InlineData("5395 0290 0902 1990")]
        [InlineData("   4978 588211036789    ")]
        public void CheckCardNumber_CardNumberIsInvalid_ReturnTrue(string cardNumber)
        {
            // Arrange
            // Act
            var cardIsValid = _cardChecker.CheckCardNumber(cardNumber);
            // Assert
            Assert.True(cardIsValid);
        }

        [Theory]
        [InlineData("1234 1234 1233 1234")]
        [InlineData("12341233123")]
        [InlineData("")]
        [InlineData(null)]
        public void CheckCardNumber_CardNumberIsInvalid_ReturnFalse(string cardNumber)
        {
            // Arrange
            // Act
            var cardIsValid = _cardChecker.CheckCardNumber(cardNumber);
            // Assert
            Assert.False(cardIsValid);
        }

        [Theory]
        [InlineData("5395029009021990")]
        [InlineData("4978588211036789")]
        public void CheckCardEmitted_CardWasNotEmittedByAlfabank_ReturnFalse(string value)
        {
            // Arrange
            // Act
            var cardWasEmittedByAlfabank = _cardChecker.CheckCardEmitter(value);
            // Assert
            Assert.False(cardWasEmittedByAlfabank);
        }

        [Theory]
        [InlineData("4083969259636239")]
        [InlineData("5101265622568232")]
        public void CheckCardEmitted_CardWasEmittedByAlfabank_ReturnTrue(string value)
        {
            // Arrange
            // Act
            var cardWasEmittedByAlfabank = _cardChecker.CheckCardEmitter(value);
            // Assert
            Assert.True(cardWasEmittedByAlfabank);
        }

        [Fact]
        public void CheckCardActivity_CardIsNull_ThrowException()
        {
            // Arrange
            void Act() => _cardChecker.CheckCardActivity(null);
            // Act
            var ex = Record.Exception((Action) Act);
            // Assert
            Assert.IsType<NotImplementedException>(ex);
        }
    }
}