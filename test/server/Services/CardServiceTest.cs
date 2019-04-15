using Moq;
using Server.Infrastructure;
using Server.Services;
using Server.Services.Checkers;
using Xunit;

namespace ServerTest.Services
{
    /// <summary>
    /// Tests for <see cref="CardService"/>>
    /// </summary>
    public class CardServiceTest
    {
        private readonly Mock<ICardChecker> _cardChecker;
        private readonly ICardService _cardService;

        public CardServiceTest()
        {
            _cardChecker = new Mock<ICardChecker>();

            _cardService = new CardService(_cardChecker.Object);
        }

        [Theory]
        [InlineData("4083969259636239", CardType.VISA)]
        [InlineData("5308276794485221", CardType.MASTERCARD)]
        [InlineData("6762302693240520", CardType.MAESTRO)]
        public void GetCardType_ReturnValidCardTypeForCardNumber(string cardNumber, CardType validCardType)
        {
            // Arrange
            _cardChecker.Setup(c => c.CheckCardNumber(cardNumber)).Returns(true);
            // Act
            var cardType = _cardService.GetCardType(cardNumber);
            // Assert
            Assert.Equal(validCardType, cardType);
        }
    }
}