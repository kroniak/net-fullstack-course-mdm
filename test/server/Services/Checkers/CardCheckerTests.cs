using Server.Services.Checkers;
using ServerTest.Mocks;
using ServerTest.Mocks.Services;
using ServerTest.Utils;
using Xunit;

namespace ServerTest.Services.Checkers
{
    /// <summary>
    /// Tests for <see cref="CardChecker"/>>
    /// </summary>
    public class CardCheckerTests
    {
        private readonly ICardChecker _cardChecker = new CardChecker();

        private readonly TestDataGenerator _testDataGenerator;

        public CardCheckerTests()
        {
            var cardService = new CardServiceMockFactory().MockObject();
            var generatorService = new CardNumberGeneratorMockFactory().MockObject();
            _testDataGenerator = new TestDataGenerator(cardService, generatorService);
        }

        /// <summary>
        /// Check if this cards numbers is valid
        /// </summary>
        [Theory]
        [InlineData("4083967629457310")]
        [InlineData("5395 0290 0902 1990")]
        [InlineData("   4978 588211036789    ")]
        public void CheckCardNumber_CorrectNumber_ReturnTrue(string value) =>
            Assert.True(_cardChecker.CheckCardNumber(value));

        [Theory]
        [InlineData("4083967629457310")]
        [InlineData("5395 0290 0902 1990")]
        [InlineData("   4978 588211036789    ")]
        [InlineData("2203572242903770")]
        public void CheckCardNumber_CardNumberIsInvalid_ReturnTrue(string cardNumber)
        {
            // Act
            var cardIsValid = _cardChecker.CheckCardNumber(cardNumber);
            // Assert
            Assert.True(cardIsValid);
        }

        [Theory]
        [InlineData("1234 1234 1233 1234")]
        [InlineData("1234 1234 1233 1234 1234 1234 1234")]
        [InlineData("12341233123")]
        [InlineData("")]
        [InlineData(null)]
        public void CheckCardNumber_CardNumberIsInvalid_ReturnFalse(string cardNumber)
        {
            // Act
            var cardIsValid = _cardChecker.CheckCardNumber(cardNumber);
            // Assert
            Assert.False(cardIsValid);
        }

        [Theory]
        [InlineData("5395029009021990")]
        [InlineData("4978588211036789")]
        [InlineData("1234 1234 1233 1234")]
        [InlineData("1234 1234 1233 1234 1234 1234 1234")]
        public void CheckCardEmitted_CardWasNotEmittedByAlfabank_ReturnFalse(string value)
        {
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
            // Act
            var cardWasEmittedByAlfabank = _cardChecker.CheckCardEmitter(value);
            // Assert
            Assert.True(cardWasEmittedByAlfabank);
        }

        [Fact]
        public void CheckCardActivity_CorrectCard_ReturnTrue()
        {
            // Arrange
            var card = _testDataGenerator.GenerateFakeCard();
            // Act
            var cardIsActivity = _cardChecker.CheckCardActivity(card);
            // Assert
            Assert.True(cardIsActivity);
        }

        [Fact]
        public void CheckCardActivity_CorrectCard_ReturnFalse()
        {
            // Arrange
            var card = _testDataGenerator.GenerateFakeValidityCard();
            // Act
            var cardActivity = _cardChecker.CheckCardActivity(card);
            // Assert
            Assert.False(cardActivity);
        }
    }
}