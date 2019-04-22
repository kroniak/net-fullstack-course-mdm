using Server.Infrastructure;
using Server.Services.Checkers;
using Server.Services.Generators;
using Server.Services.Interfaces;
using Server.Services;
using Server.Services.Converters;
using Xunit;

namespace ServerTest.Services.Generators
{
    public class AlfaCardNumberGeneratorTests
    {
        private readonly ICardChecker _cardChecker;
        private readonly ICardNumberGenerator _cardNumberGenerator;
        private readonly ICardService _cardService;

        public AlfaCardNumberGeneratorTests()
        {
            _cardChecker = new CardChecker();
            _cardService = new CardService(_cardChecker, new CurrencyConverter());
            _cardNumberGenerator = new AlfaCardNumberGenerator();
        }

        [Theory]
        [InlineData(CardType.MAESTRO)]
        [InlineData(CardType.MASTERCARD)]
        [InlineData(CardType.MIR)]
        [InlineData(CardType.VISA)]
        public void GenerateNewCardNumber_CorrectCardType_ReturnCorrectCardNumber(CardType type)
        {
            // Act
            var cardNumber = _cardNumberGenerator.GenerateNewCardNumber(type);
            var validationResult = _cardChecker.CheckCardEmitter(cardNumber);

            // Assert
            Assert.True(validationResult);
        }

        [Theory]
        [InlineData(CardType.MAESTRO)]
        [InlineData(CardType.MASTERCARD)]
        [InlineData(CardType.MIR)]
        [InlineData(CardType.VISA)]
        public void GenerateNewCardNumber_CorrectCardType_ReturnCorrectCardType(CardType type)
        {
            // Act
            var cardNumber = _cardNumberGenerator.GenerateNewCardNumber(type);
            var cardType = _cardService.GetCardType(cardNumber);

            // Assert
            Assert.Equal(type, cardType);
        }

        [Fact]
        public void GenerateNewCardNumber_IncorrectCardType_ReturnNull()
        {
            // Act
            var result = _cardNumberGenerator.GenerateNewCardNumber(CardType.OTHER);

            // Assert
            Assert.Null(result);
        }
    }
}