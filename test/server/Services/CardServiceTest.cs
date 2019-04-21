using Moq;
using Server.Infrastructure;
using Server.Models;
using Server.Services;
using Server.Services.Checkers;
using Server.Services.Converters;
using Xunit;

namespace ServerTest.Services
{
    /// <summary>
    /// Tests for <see cref="CardService"/>>
    /// </summary>
    public class CardServiceTest
    {
        private readonly ICardService _cardService;

        public CardServiceTest()
        {
            var cardCheckerMock = new Mock<ICardChecker>();
            var currencyConverterMock = new Mock<ICurrencyConverter>();
            _cardService = new CardService(cardCheckerMock.Object);
        }

        [Theory]
        [InlineData("4083969259636239", CardType.VISA)]
        [InlineData("5308276794485221", CardType.MASTERCARD)]
        [InlineData("6762302693240520", CardType.MAESTRO)]
        [InlineData("6762502693240520", CardType.OTHER)]
        public void GetCardType_ReturnValidCardTypeForCardNumber(string cardNumber, CardType validCardType)
        {
            // Arrange
            // Act
            var cardType = _cardService.GetCardType(cardNumber);
            // Assert
            Assert.Equal(validCardType, cardType);
        }

        [Theory]
        [InlineData(Currency.RUR, "10")]
        [InlineData(Currency.EUR, "0,1376651982378854625550660793")]
        [InlineData(Currency.USD, "0,1595405232929164007657945118")]
        public void AddBonusOnOpen_CardBalanceContainsBonus(Currency cardCurrency, string valueOut)
        {
            // Arrange
            var validBalance = System.Convert.ToDecimal(valueOut);
            var card = new Card
            {
                CardNumber = "4790878827491205",
                Currency = cardCurrency
            };
            // Act
            var addBonusOnOpenResult = _cardService.TryAddBonusOnOpen(card);
            // Assert
            var cardBalanceAfterAddingOfBonus = _cardService.GetBalanceOfCard(card);
            Assert.Equal(validBalance, cardBalanceAfterAddingOfBonus);
            Assert.True(addBonusOnOpenResult);
        }
    }
}