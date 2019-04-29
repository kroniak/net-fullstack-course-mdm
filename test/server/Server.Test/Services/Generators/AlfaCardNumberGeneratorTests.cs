using AlfaBank.Core.Exceptions;
using AlfaBank.Core.Infrastructure;
using AlfaBank.Services;
using AlfaBank.Services.Checkers;
using AlfaBank.Services.Converters;
using AlfaBank.Services.Generators;
using AlfaBank.Services.Interfaces;
using Xunit;

namespace Server.Test.Services.Generators
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

        [Fact]
        public void GenerateNewCardNumber_NotFoundStartBin_ThrowException()
        {
            // Arrange
            var temp = Constants.AlfaBins[0];
            Constants.AlfaBins[0] = string.Empty;

            // Assert
            Assert.Throws<CriticalException>(() => _cardNumberGenerator.GenerateNewCardNumber(CardType.MIR));

            // Revert
            Constants.AlfaBins[0] = temp;
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