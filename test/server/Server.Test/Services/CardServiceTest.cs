using AlfaBank.Core.Infrastructure;
using AlfaBank.Core.Models;
using AlfaBank.Services;
using AlfaBank.Services.Converters;
using AlfaBank.Services.Interfaces;
using Moq;
using Server.Test.Mocks;
using Server.Test.Utils;
using System;
using System.Collections.Generic;
using AlfaBank.Core.Data.Interfaces;
using Xunit;

namespace Server.Test.Services
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

            _currencyConverterMock.Setup(
                    c => c.GetConvertedSum(
                        It.IsAny<decimal>(),
                        It.IsAny<Currency>(),
                        It.IsAny<Currency>()))
                .Returns(10M);

            _cardService = new CardService(
                cardCheckerMock.Object,
                _currencyConverterMock.Object,
                new Mock<ICardRepository>().Object);
            _testDataGenerator = new TestDataGenerator(_cardService, cardNumberGenerator);
        }

        [Theory]
        [InlineData("4083969259636239", CardType.VISA)]
        [InlineData("5308276794485221", CardType.MASTERCARD)]
        [InlineData("6762302693240520", CardType.MAESTRO)]
        [InlineData("5659212290902737", CardType.MAESTRO)]
        [InlineData("2203572242903770", CardType.MIR)]
        [InlineData("6762502693240520", CardType.OTHER)]
        [InlineData("346596311028062", CardType.OTHER)]
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
                CardNumber = "4790878827491205",
                Transactions = new List<Transaction>()
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

            _currencyConverterMock.Setup(
                c => c.GetConvertedSum(
                    It.IsAny<decimal>(),
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

        [Fact]
        public void TryTariffCharge_ValidCard_ValidTariff_BalanceChanged_True()
        {
            // Arrange
            const decimal tariff = 0.1M;
            var card = _testDataGenerator.GenerateFakeCard();
            var balance = card.Balance;

            _currencyConverterMock.Setup(
                    c => c.GetConvertedSum(
                        tariff,
                        It.IsAny<Currency>(),
                        It.IsAny<Currency>()))
                .Returns(tariff);

            // Act
            var result = _cardService.TryTariffCharge(card, tariff);

            // Assert
            _currencyConverterMock.Verify(
                c => c.GetConvertedSum(
                    tariff,
                    It.IsAny<Currency>(),
                    It.IsAny<Currency>()), Times.Once);

            Assert.Equal(balance - tariff, card.Balance);
            Assert.True(result);
        }

        [Fact]
        public void TryTariffCharge_ValidCard_ValidTariff_TransactionCountIncrease()
        {
            // Arrange
            const decimal tariff = 0.1M;
            var card = _testDataGenerator.GenerateFakeCard();
            var count = card.Transactions.Count;

            _currencyConverterMock.Setup(
                    c => c.GetConvertedSum(
                        tariff,
                        It.IsAny<Currency>(),
                        It.IsAny<Currency>()))
                .Returns(tariff);

            // Act
            _cardService.TryTariffCharge(card, tariff);

            // Assert
            Assert.Equal(count + 1, card.Transactions.Count);
        }

        [Theory]
        [InlineData(-0.1)]
        [InlineData(0)]
        public void TryTariffCharge_ValidCard_InvalidTariff_BalanceNotChanged_ReturnFalse(decimal tariff)
        {
            // Arrange
            var card = _testDataGenerator.GenerateFakeCard();
            var balance = card.Balance;

            _currencyConverterMock.Setup(
                    c => c.GetConvertedSum(
                        tariff,
                        It.IsAny<Currency>(),
                        It.IsAny<Currency>()))
                .Returns(tariff);

            // Act
            var result = _cardService.TryTariffCharge(card, tariff);

            // Assert
            _currencyConverterMock.Verify(
                c => c.GetConvertedSum(
                    tariff,
                    It.IsAny<Currency>(),
                    It.IsAny<Currency>()), Times.Never);

            Assert.Equal(balance, card.Balance);
            Assert.False(result);
        }

        [Fact]
        public void TryTariffCharge_ValidCard_HighTariff_BalanceNotChanged_ReturnFalse()
        {
            // Arrange
            const decimal tariff = 100M;
            var card = _testDataGenerator.GenerateFakeCard();
            var balance = card.Balance;

            _currencyConverterMock.Setup(
                    c => c.GetConvertedSum(
                        tariff,
                        It.IsAny<Currency>(),
                        It.IsAny<Currency>()))
                .Returns(tariff);

            // Act
            var result = _cardService.TryTariffCharge(card, tariff);

            // Assert
            _currencyConverterMock.Verify(
                c => c.GetConvertedSum(
                    tariff,
                    It.IsAny<Currency>(),
                    It.IsAny<Currency>()), Times.Once);

            Assert.Equal(balance, card.Balance);
            Assert.False(result);
        }

        [Fact]
        public void TryTariffCharge_ValidCard_HighTariff_TransactionCountNotIncrease()
        {
            // Arrange
            const decimal tariff = 100M;
            var card = _testDataGenerator.GenerateFakeCard();
            var count = card.Transactions.Count;

            _currencyConverterMock.Setup(
                    c => c.GetConvertedSum(
                        tariff,
                        It.IsAny<Currency>(),
                        It.IsAny<Currency>()))
                .Returns(tariff);

            // Act
            _cardService.TryTariffCharge(card, tariff);

            // Assert
            Assert.Equal(count, card.Transactions.Count);
        }

        [Fact]
        public void TryTariffCharge_ValidCard_HighTariff_ThrowException_ReturnFalse()
        {
            // Arrange
            const decimal tariff = 0.1M;
            var card = _testDataGenerator.GenerateFakeCard();

            _currencyConverterMock.Setup(
                    c => c.GetConvertedSum(
                        tariff,
                        It.IsAny<Currency>(),
                        It.IsAny<Currency>()))
                .Throws<Exception>();

            // Act
            var result = _cardService.TryTariffCharge(card, tariff);

            // Assert
            _currencyConverterMock.Verify(
                c => c.GetConvertedSum(
                    tariff,
                    It.IsAny<Currency>(),
                    It.IsAny<Currency>()), Times.Once);

            Assert.False(result);
        }

        [Fact]
        public void TryTariffCharge_ValidCard_HighTariff_ThrowException_BalanceNotChanged()
        {
            // Arrange
            const decimal tariff = 0.1M;
            var card = _testDataGenerator.GenerateFakeCard();
            var balance = card.Balance;

            _currencyConverterMock.Setup(
                    c => c.GetConvertedSum(
                        tariff,
                        It.IsAny<Currency>(),
                        It.IsAny<Currency>()))
                .Throws<Exception>();

            // Act
            _cardService.TryTariffCharge(card, tariff);

            // Assert
            Assert.Equal(balance, card.Balance);
        }

        [Fact]
        public void TryTariffCharge_ValidCard_HighTariff_ThrowException_TransactionCountNotIncrease()
        {
            // Arrange
            const decimal tariff = 0.1M;
            var card = _testDataGenerator.GenerateFakeCard();
            var count = card.Transactions.Count;

            _currencyConverterMock.Setup(
                    c => c.GetConvertedSum(
                        tariff,
                        It.IsAny<Currency>(),
                        It.IsAny<Currency>()))
                .Throws<Exception>();

            // Act
            _cardService.TryTariffCharge(card, tariff);

            // Assert
            Assert.Equal(count, card.Transactions.Count);
        }

        [Theory]
        [InlineData(-0.1)]
        [InlineData(0)]
        public void TryTariffCharge_ValidCard_InvalidTariff_TransactionCountNotIncrease(decimal tariff)
        {
            // Arrange
            var card = _testDataGenerator.GenerateFakeCard();
            var count = card.Transactions.Count;

            _currencyConverterMock.Setup(
                    c => c.GetConvertedSum(
                        tariff,
                        It.IsAny<Currency>(),
                        It.IsAny<Currency>()))
                .Returns(tariff);

            // Act
            _cardService.TryTariffCharge(card, tariff);

            // Assert
            Assert.Equal(count, card.Transactions.Count);
        }
    }
}