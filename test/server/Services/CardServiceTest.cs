using Moq;
using Server.Infrastructure;
using Server.Models;
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
        private readonly ICardService _cardService;
        private readonly Mock<ICardChecker> _cardCheckerMock;

        public CardServiceTest()
        {
            _cardCheckerMock = new Mock<ICardChecker>();
            _cardService = new CardService(_cardCheckerMock.Object);
        }

        [Theory]
        [InlineData("4083969259636239", CardType.VISA)]
        [InlineData("5308276794485221", CardType.MASTERCARD)]
        [InlineData("6762302693240520", CardType.MAESTRO)]
        [InlineData("9762302693240524", CardType.OTHER)]
        public void GetCardType_ReturnValidCardTypeForCardNumber(string cardNumber, CardType validCardType)
        {
			// Arrange
			_cardCheckerMock
				.Setup(x => x.CheckCardNumber(cardNumber))
				.Returns(true);

			// Act
			var cardType = _cardService.GetCardType(cardNumber);
            // Assert
            Assert.Equal(validCardType, cardType);
        }

        [Fact]
        public void AddBonusOnOpen_CardBalanceContainsBonus()
        {
            // Arrange
            const decimal validCardBalanceAfterAddingOfBonus = 10M;
            var card = new Card
            {
                CardName = "4790878827491205"
            };
            // Act
            var addBonusOnOpenResult = _cardService.TryAddBonusOnOpen(card);
            // Assert
            var cardBalanceAfterAddingOfBonus = _cardService.GetBalanceOfCard(card);
            Assert.Equal(validCardBalanceAfterAddingOfBonus, cardBalanceAfterAddingOfBonus);
            Assert.True(addBonusOnOpenResult);
        }

		[Theory]
		[InlineData(CardType.VISA)]
		[InlineData(CardType.MASTERCARD)]
		[InlineData(CardType.MAESTRO)]
		public void GenerateNewCardNumber_ValidCardNumber(CardType cardType)
		{
			// Act
			var newCardNumber = _cardService.GenerateNewCardNumber(cardType);

			_cardCheckerMock
				.Setup(x => x.CheckCardNumber(newCardNumber))
				.Returns(true);
			var typeNewCard = _cardService.GetCardType(newCardNumber);
			// Assert
			Assert.Equal(typeNewCard, cardType);
			Assert.NotNull(newCardNumber);
			Assert.Equal(16, newCardNumber.Length);
		}
	}
}