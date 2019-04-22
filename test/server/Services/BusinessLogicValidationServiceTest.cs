using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Moq;
using Server.Services;
using Server.Services.Checkers;
using Server.Services.Interfaces;
using ServerTest.Mocks;
using ServerTest.Mocks.Services;
using ServerTest.Utils;
using Xunit;

namespace ServerTest.Services
{
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public class BusinessLogicValidationServiceTest
    {
        private readonly Mock<ICardChecker> _cardCheckerMock;
        private readonly IBusinessLogicValidationService _businessLogicValidationService;
        private readonly TestDataGenerator _testDataGenerator;

        public BusinessLogicValidationServiceTest()
        {
            var cardService = new CardServiceMockFactory().MockObject();
            _cardCheckerMock = new CardCheckerMockFactory().Mock();
            var generatorService = new CardNumberGeneratorMockFactory().MockObject();

            _businessLogicValidationService =
                new BusinessLogicValidationService(_cardCheckerMock.Object);

            _testDataGenerator = new TestDataGenerator(cardService, generatorService);
        }

        [Fact]
        public void ValidateTransfer_ReturnEmptyErrorsList()
        {
            // Arrange
            var cards = _testDataGenerator.GenerateFakeCards();
            var card = cards.First();

            _cardCheckerMock.Setup(x => x.CheckCardActivity(card)).Returns(true);
            _cardCheckerMock.Setup(x => x.CheckCardActivity(cards.ElementAt(1))).Returns(true);

            // Act
            var result = _businessLogicValidationService.ValidateTransfer(card, cards.ElementAt(1), 1);

            // Assert
            _cardCheckerMock.Verify(x => x.CheckCardActivity(card), Times.Once);
            _cardCheckerMock.Verify(x => x.CheckCardActivity(cards.ElementAt(1)), Times.Once);

            Assert.Empty(result);
        }

        [Fact]
        public void ValidateTransfer_EqualCards_ReturnSingleError()
        {
            // Arrange
            var card = _testDataGenerator.GenerateFakeCards().First();

            _cardCheckerMock.Setup(x => x.CheckCardActivity(card)).Returns(true);

            // Act
            var result = _businessLogicValidationService.ValidateTransfer(card, card, 1);

            // Assert
            _cardCheckerMock.Verify(x => x.CheckCardActivity(card), Times.AtMost(2));

            Assert.Single(result);
            Assert.Equal("from", result.First().FieldName);
            Assert.Equal("From card and to card is Equal", result.First().Message);
            Assert.Equal("Нельзя перевести на ту же карту", result.First().LocalizedMessage);
        }

        [Fact]
        public void ValidateTransfer_FromCardExpire_ReturnSingleError()
        {
            // Arrange
            var card1 = _testDataGenerator.GenerateFakeValidityCard();
            var card2 = _testDataGenerator.GenerateFakeCard();

            _cardCheckerMock.Setup(x => x.CheckCardActivity(card1)).Returns(false);
            _cardCheckerMock.Setup(x => x.CheckCardActivity(card2)).Returns(true);

            // Act
            var result = _businessLogicValidationService.ValidateTransfer(card1, card2, 1);

            // Assert
            _cardCheckerMock.Verify(x => x.CheckCardActivity(card1), Times.Once);
            _cardCheckerMock.Verify(x => x.CheckCardActivity(card2), Times.Once);

            Assert.Single(result);
            Assert.Equal("from", result.First().FieldName);
            Assert.Equal("Card is expired", result.First().Message);
            Assert.Equal("Карта просрочена", result.First().LocalizedMessage);
        }

        [Fact]
        public void ValidateTransfer_ToCardExpire_ReturnSingleError()
        {
            // Arrange
            var card1 = _testDataGenerator.GenerateFakeCard();
            var card2 = _testDataGenerator.GenerateFakeValidityCard();

            _cardCheckerMock.Setup(x => x.CheckCardActivity(card1)).Returns(true);
            _cardCheckerMock.Setup(x => x.CheckCardActivity(card2)).Returns(false);

            // Act
            var result = _businessLogicValidationService.ValidateTransfer(card1, card2, 1);

            // Assert
            _cardCheckerMock.Verify(x => x.CheckCardActivity(card1), Times.Once);
            _cardCheckerMock.Verify(x => x.CheckCardActivity(card2), Times.Once);

            Assert.Single(result);
            Assert.Equal("to", result.First().FieldName);
            Assert.Equal("Card is expired", result.First().Message);
            Assert.Equal("Карта просрочена", result.First().LocalizedMessage);
        }

        [Fact]
        public void ValidateTransfer_LowMoney_ReturnSingleError()
        {
            // Arrange
            var card1 = _testDataGenerator.GenerateFakeCard();
            var card2 = _testDataGenerator.GenerateFakeCard();

            _cardCheckerMock.Setup(x => x.CheckCardActivity(card1)).Returns(true);
            _cardCheckerMock.Setup(x => x.CheckCardActivity(card2)).Returns(true);

            // Act
            var result = _businessLogicValidationService.ValidateTransfer(card1, card2, 10000);

            // Assert
            _cardCheckerMock.Verify(x => x.CheckCardActivity(card1), Times.Once);
            _cardCheckerMock.Verify(x => x.CheckCardActivity(card2), Times.Once);
            Assert.Single(result);
            Assert.Equal("from", result.First().FieldName);
            Assert.Equal("Balance of the card is low", result.First().Message);
            Assert.Equal("Нет денег на карте", result.First().LocalizedMessage);
        }

        [Fact]
        public void ValidateCardExist_ReturnTrue()
        {
            // Arrange
            var cards = _testDataGenerator.GenerateFakeCards();
            var card = cards.First();
            // Act
            var result = _businessLogicValidationService.ValidateCardExist(cards, card.CardName, card.CardNumber);
            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ValidateCardExist_ReturnFalse()
        {
            // Arrange
            var cards = _testDataGenerator.GenerateFakeCards();
            var card = _testDataGenerator.GenerateFakeCard();
            // Act
            var result = _businessLogicValidationService.ValidateCardExist(cards, card.CardName, card.CardNumber);
            // Assert
            Assert.False(result);
        }
    }
}