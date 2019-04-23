using System;
using Moq;
using Server.Infrastructure;
using Server.Models;
using Server.Services;
using Server.Services.Converters;
using Server.Services.Interfaces;
using ServerTest.Mocks;
using ServerTest.Utils;
using Xunit;

namespace ServerTest.Services
{
    /// <summary>
    /// Tests for <see cref="CardService"/>>
    /// </summary>
    public class CardServiceTest
    {
        private readonly ICardService _cardService;
        private readonly TestDataGenerator _testDataGenerator;
        private readonly Mock<ICurrencyConverter> _currencyConverterMock;

        public CardServiceTest()
        {
            var cardNumberGenerator = new CardNumberGeneratorMockFactory().MockObject();
            var cardCheckerMock = new CardCheckerMockFactory().Mock();
            _currencyConverterMock = new Mock<ICurrencyConverter>();

            _currencyConverterMock.Setup(c => c.GetConvertedSum(
                    It.IsAny<decimal>(),
                    It.IsAny<Currency>(),
                    It.IsAny<Currency>()))
                .Returns(10M);

            _cardService = new CardService(cardCheckerMock.Object, _currencyConverterMock.Object);
            _testDataGenerator = new TestDataGenerator(_cardService, cardNumberGenerator);
        }

        [Theory]
        [InlineData("4083969259636239", CardType.VISA)]
        [InlineData("5308276794485221", CardType.MASTERCARD)]
        [InlineData("6762302693240520", CardType.MAESTRO)]
        [InlineData("2203572242903770", CardType.MIR)]
        [InlineData("6762502693240520", CardType.OTHER)]
        public void GetCardType_ReturnValidCardTypeForCardNumber(string cardNumber, CardType validCardType)
        {
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
                CardNumber = "4790878827491205"
            };
            // Act
            var addBonusOnOpenResult = _cardService.TryAddBonusOnOpen(card);
            // Assert
            var cardBalanceAfterAddingOfBonus = card.Balance;
            Assert.Equal(validCardBalanceAfterAddingOfBonus, cardBalanceAfterAddingOfBonus);
            Assert.True(addBonusOnOpenResult);
        }

        [Fact]
        public void TryAddBonusOnOpen_MoneyAddedSuccessfully()
        {
            // Arrange
            var card = _testDataGenerator.GenerateFakeCard();
            card.Transactions.Clear();

            // Act
            var result = _cardService.TryAddBonusOnOpen(card);
            var balanceOfCard = card.Balance;

            // Assert
            Assert.True(result);
            Assert.Equal(10M, balanceOfCard);
        }

        [Fact]
        public void TryAddBonusOnOpen_SuccessOneTransaction()
        {
            // Arrange
            var card = _testDataGenerator.GenerateFakeCard();
            card.Transactions.Clear();

            // Act
            var result = _cardService.TryAddBonusOnOpen(card);

            // Assert
            Assert.True(result);
            Assert.Single(card.Transactions);
        }

        [Fact]
        public void TryAddBonusOnOpen_Exception_ReturnFalse()
        {
            // Arrange
            var card = _testDataGenerator.GenerateFakeCard();
            card.Transactions.Clear();

            _currencyConverterMock.Setup(c => c.GetConvertedSum(It.IsAny<decimal>(),
                It.IsAny<Currency>(),
                It.IsAny<Currency>())).Throws<Exception>();

            // Act
            var result = _cardService.TryAddBonusOnOpen(card);

            // Assert
            Assert.False(result);
            Assert.Empty(card.Transactions);
        }

        [Fact]
        public void GetBalanceOfCard_NewCard_ReturnCorrectBalance()
        {
            // Arrange
            var card = _testDataGenerator.GenerateFakeCard();
            // Act
            var balanceOfCard = card.RoundBalance;
            // Assert
            Assert.Equal(10M, balanceOfCard);
        }
    }
}