using System.Collections.Generic;
using System.IO;
using System.Linq;
using Moq;
using Server.Data.Interfaces;
using Server.Exceptions;
using Server.Infrastructure;
using Server.Models;
using Server.Services;
using Server.Services.Converters;
using Server.Services.Interfaces;
using ServerTest.Mocks;
using ServerTest.Mocks.Data;
using ServerTest.Mocks.Services;
using ServerTest.Utils;
using Xunit;

// ReSharper disable PossibleMultipleEnumeration
// ReSharper disable ImplicitlyCapturedClosure

namespace ServerTest.Services
{
    public class BankServiceTest
    {
        private readonly Mock<ICardService> _cardServiceMock;
        private readonly Mock<ICardRepository> _cardRepositoryMock;
        private readonly Mock<ICurrencyConverter> _currencyConverterMock;

        private readonly Mock<IBusinessLogicValidationService> _validationBlServiceMock;
        private readonly IEnumerable<Card> _cards;
        private readonly User _user;
        private readonly IBankService _bankService;
        private readonly Mock<ICardNumberGenerator> _cardNumberGeneratorMock;

        public BankServiceTest()
        {
            _cardServiceMock = new CardServiceMockFactory().Mock();
            _cardNumberGeneratorMock = new CardNumberGeneratorMockFactory().Mock();
            _validationBlServiceMock = new BusinessLogicValidationServiceMockFactory().Mock();
            _currencyConverterMock = new Mock<ICurrencyConverter>();

            var testDataGenerator = new TestDataGenerator(
                _cardServiceMock.Object,
                _cardNumberGeneratorMock.Object);

            _cards = testDataGenerator.GenerateFakeCards();
            _user = TestDataGenerator.GenerateFakeUser(_cards);

            _cardRepositoryMock = new CardsRepositoryMockFactory(_user).Mock();

            _bankService = new BankService(
                _cardRepositoryMock.Object,
                _cardServiceMock.Object,
                _validationBlServiceMock.Object,
                _currencyConverterMock.Object,
                _cardNumberGeneratorMock.Object
            );
        }

        [Theory]
        [InlineData(CardType.MASTERCARD)]
        [InlineData(CardType.VISA)]
        [InlineData(CardType.MAESTRO)]
        [InlineData(CardType.MIR)]
        public void TryOpenNewCard_CorrectTypeData_ReturnEmptyErrorsList(CardType type)
        {
            // Arrange
            _validationBlServiceMock.Setup(b => b.ValidateCardExist(
                It.IsAny<IEnumerable<Card>>(), "name", It.IsAny<string>())).Returns(false);

            // Act
            var (_, errors) = _bankService.TryOpenNewCard(_user, "name", Currency.RUR, type);

            // Assert
            _cardRepositoryMock.Verify(c => c.GetCards(_user), Times.Once);
            _cardNumberGeneratorMock.Verify(c => c.GenerateNewCardNumber(type), Times.AtMost(2));
            _validationBlServiceMock.Verify(b => b.ValidateCardExist(
                It.IsAny<IEnumerable<Card>>(), "name", It.IsAny<string>()), Times.Once);
            _cardServiceMock.Verify(x => x.TryAddBonusOnOpen(It.IsAny<Card>()), Times.AtMost(4));

            Assert.Empty(errors);
        }

        [Theory]
        [InlineData(CardType.MASTERCARD)]
        [InlineData(CardType.VISA)]
        [InlineData(CardType.MAESTRO)]
        [InlineData(CardType.MIR)]
        public void TryOpenNewCard_CorrectData_ReturnCorrectCard(CardType type)
        {
            // Arrange
            _validationBlServiceMock.Setup(b => b.ValidateCardExist(
                It.IsAny<IEnumerable<Card>>(), "name", It.IsAny<string>())).Returns(false);

            // Act
            var (card, _) = _bankService.TryOpenNewCard(_user, "name", Currency.RUR, type);

            // Assert
            _cardRepositoryMock.Verify(c => c.GetCards(_user), Times.Once);
            _cardNumberGeneratorMock.Verify(c => c.GenerateNewCardNumber(type), Times.AtMost(2));
            _validationBlServiceMock.Verify(b => b.ValidateCardExist(
                It.IsAny<IEnumerable<Card>>(), "name", It.IsAny<string>()), Times.Once);
            _cardServiceMock.Verify(x => x.TryAddBonusOnOpen(It.IsAny<Card>()), Times.AtMost(4));

            Assert.Equal(Currency.RUR, card.Currency);
            Assert.Single(card.Transactions);
            Assert.Equal(type, card.CardType);
            Assert.Equal("name", card.CardName);
        }

        [Fact]
        public void TryOpenNewCard_CardTypeOther_ReturnNullCard()
        {
            // Act
            var (card, _) = _bankService.TryOpenNewCard(_user, "name", Currency.RUR, CardType.OTHER);

            // Assert
            _cardRepositoryMock.Verify(c => c.GetCards(_user), Times.Never);
            _cardNumberGeneratorMock.Verify(c => c.GenerateNewCardNumber(CardType.OTHER), Times.Never);
            _validationBlServiceMock.Verify(b => b.ValidateCardExist(
                It.IsAny<IEnumerable<Card>>(), "name", It.IsAny<string>()), Times.Never);
            _cardServiceMock.Verify(x => x.TryAddBonusOnOpen(It.IsAny<Card>()), Times.AtMost(3));

            Assert.Null(card);
        }

        [Fact]
        public void TryOpenNewCard_CardTypeOther_ReturnError()
        {
            // Act
            var (_, errors) = _bankService.TryOpenNewCard(_user, "name", Currency.RUR, CardType.OTHER);

            // Assert
            _cardRepositoryMock.Verify(c => c.GetCards(_user), Times.Never);
            _cardNumberGeneratorMock.Verify(c => c.GenerateNewCardNumber(CardType.OTHER), Times.Never);
            _validationBlServiceMock.Verify(b => b.ValidateCardExist(
                It.IsAny<IEnumerable<Card>>(), "name", It.IsAny<string>()), Times.Never);
            _cardServiceMock.Verify(x => x.TryAddBonusOnOpen(It.IsAny<Card>()), Times.AtMost(3));

            Assert.Equal("type", errors.First().FieldName);
            Assert.Equal("Wrong type card", errors.First().Message);
            Assert.Equal("Неверный тип карты", errors.First().LocalizedMessage);
        }

        [Fact]
        public void TryOpenNewCard_ExistingCard_ReturnError()
        {
            // Assert 
            _validationBlServiceMock.Setup(b => b.ValidateCardExist(
                It.IsAny<IEnumerable<Card>>(), "name", It.IsAny<string>())).Returns(true);

            // Act
            var (_, errors) = _bankService.TryOpenNewCard(_user, "name", Currency.RUR, CardType.MASTERCARD);

            // Assert
            _cardRepositoryMock.Verify(c => c.GetCards(_user), Times.Once);
            _cardNumberGeneratorMock.Verify(c => c.GenerateNewCardNumber(CardType.MASTERCARD), Times.AtMost(4));
            _validationBlServiceMock.Verify(b => b.ValidateCardExist(
                It.IsAny<IEnumerable<Card>>(), "name", It.IsAny<string>()), Times.Once);
            _cardServiceMock.Verify(x => x.TryAddBonusOnOpen(It.IsAny<Card>()), Times.AtMost(3));

            Assert.NotEmpty(errors);
            Assert.Equal("internal", errors.First().FieldName);
            Assert.Equal("Card exist", errors.First().Message);
            Assert.Equal("Карта с таким номером уже существует", errors.First().LocalizedMessage);
        }

        [Fact]
        public void TryOpenNewCard_ExistingCard_ReturnNullCard()
        {
            // Assert 
            _validationBlServiceMock.Setup(b => b.ValidateCardExist(
                It.IsAny<IEnumerable<Card>>(), "name", It.IsAny<string>())).Returns(true);

            // Act
            var (card, _) = _bankService.TryOpenNewCard(_user, "name", Currency.RUR, CardType.MASTERCARD);

            // Assert
            _cardRepositoryMock.Verify(c => c.GetCards(_user), Times.Once);
            _cardNumberGeneratorMock.Verify(c => c.GenerateNewCardNumber(CardType.MASTERCARD), Times.AtMost(4));
            _validationBlServiceMock.Verify(b => b.ValidateCardExist(
                It.IsAny<IEnumerable<Card>>(), "name", It.IsAny<string>()), Times.Once);
            _cardServiceMock.Verify(x => x.TryAddBonusOnOpen(It.IsAny<Card>()), Times.AtMost(3));

            Assert.Null(card);
        }

        [Fact]
        public void TryOpenNewCard_TryAddBonusInternalError_ReturnError()
        {
            // Assert 
            _validationBlServiceMock.Setup(b => b.ValidateCardExist(
                It.IsAny<IEnumerable<Card>>(), "name", It.IsAny<string>())).Returns(false);

            _cardServiceMock.Setup(b => b.TryAddBonusOnOpen(It.IsAny<Card>())).Returns(false);

            // Act
            var (_, errors) = _bankService.TryOpenNewCard(_user, "name", Currency.RUR, CardType.MASTERCARD);

            // Assert
            _cardRepositoryMock.Verify(c => c.GetCards(_user), Times.Once);
            _cardNumberGeneratorMock.Verify(c => c.GenerateNewCardNumber(CardType.MASTERCARD), Times.AtMost(4));
            _validationBlServiceMock.Verify(b => b.ValidateCardExist(
                It.IsAny<IEnumerable<Card>>(), "name", It.IsAny<string>()), Times.Once);
            _cardServiceMock.Verify(x => x.TryAddBonusOnOpen(It.IsAny<Card>()), Times.AtMost(4));

            Assert.NotEmpty(errors);
            Assert.Equal("internal", errors.First().FieldName);
            Assert.Equal("Add bonus to card failed", errors.First().Message);
            Assert.Equal("Ошибка при открытии карты", errors.First().LocalizedMessage);
        }

        [Fact]
        public void TryOpenNewCard_TryAddBonusException_ReturnError()
        {
            // Assert 
            _validationBlServiceMock.Setup(b => b.ValidateCardExist(
                It.IsAny<IEnumerable<Card>>(), "name", It.IsAny<string>())).Returns(false);

            _cardServiceMock.Setup(b => b.TryAddBonusOnOpen(It.IsAny<Card>())).Throws<IOException>();

            // Act
            var (_, errors) = _bankService.TryOpenNewCard(_user, "name", Currency.RUR, CardType.MASTERCARD);

            // Assert
            _cardRepositoryMock.Verify(c => c.GetCards(_user), Times.Once);
            _cardNumberGeneratorMock.Verify(c => c.GenerateNewCardNumber(CardType.MASTERCARD), Times.AtMost(4));
            _validationBlServiceMock.Verify(b => b.ValidateCardExist(
                It.IsAny<IEnumerable<Card>>(), "name", It.IsAny<string>()), Times.Once);
            _cardServiceMock.Verify(x => x.TryAddBonusOnOpen(It.IsAny<Card>()), Times.AtMost(4));

            Assert.NotEmpty(errors);
            Assert.Equal("internal", errors.First().FieldName);
            Assert.Equal("I/O error occurred.", errors.First().Message);
            Assert.Equal("Что то пошло не так", errors.First().LocalizedMessage);
        }

        [Fact]
        public void TryOpenNewCard_TryAddBonusException_ReturnNullCard()
        {
            // Assert 
            _validationBlServiceMock.Setup(b => b.ValidateCardExist(
                It.IsAny<IEnumerable<Card>>(), "name", It.IsAny<string>())).Returns(false);

            _cardServiceMock.Setup(b => b.TryAddBonusOnOpen(It.IsAny<Card>())).Throws<IOException>();

            // Act
            var (card, _) = _bankService.TryOpenNewCard(_user, "name", Currency.RUR, CardType.MASTERCARD);

            // Assert
            _cardRepositoryMock.Verify(c => c.GetCards(_user), Times.Once);

            _cardNumberGeneratorMock.Verify(c => c.GenerateNewCardNumber(CardType.MASTERCARD), Times.AtMost(4));
            _validationBlServiceMock.Verify(b => b.ValidateCardExist(
                It.IsAny<IEnumerable<Card>>(), "name", It.IsAny<string>()), Times.Once);
            _cardServiceMock.Verify(x => x.TryAddBonusOnOpen(It.IsAny<Card>()), Times.AtMost(4));

            Assert.Null(card);
        }

        [Fact]
        public void TryTransferMoney_CorrectData_ReturnEmptyErrorsList()
        {
            // Arrange
            var from = _cards.First();
            var to = _cards.ElementAt(1);

            _cardRepositoryMock.Setup(c => c.GetCard(_user, from.CardNumber)).Returns(from);
            _cardRepositoryMock.Setup(c => c.GetCard(_user, to.CardNumber)).Returns(to);

            const decimal sum = 10M;

            _validationBlServiceMock.Setup(b => b.ValidateTransfer(from, to, sum))
                .Returns(new List<CustomModelError>());

            _currencyConverterMock.Setup(b => b.GetConvertedSum(sum, from.Currency, to.Currency))
                .Returns(10);

            // Act
            var (_, errors) = _bankService.TryTransferMoney(_user, sum, from.CardNumber, to.CardNumber);

            // Assert
            _cardRepositoryMock.Verify(c => c.GetCard(_user, from.CardNumber), Times.Once);
            _cardRepositoryMock.Verify(c => c.GetCard(_user, to.CardNumber), Times.Once);
            _validationBlServiceMock.Verify(b => b.ValidateTransfer(from, to, sum), Times.Once);
            _currencyConverterMock.Verify(b => b.GetConvertedSum(sum, from.Currency, to.Currency),
                Times.Once);

            Assert.Empty(errors);
        }

        [Fact]
        public void TryTransferMoney_CorrectData_ReturnCorrectTransaction()
        {
            // Arrange
            var from = _cards.First();
            var to = _cards.ElementAt(1);

            var fromBalance = from.Balance;
            var toBalance = to.Balance;

            _cardRepositoryMock.Setup(c => c.GetCard(_user, from.CardNumber)).Returns(from);
            _cardRepositoryMock.Setup(c => c.GetCard(_user, to.CardNumber)).Returns(to);

            const decimal sum = 10M;

            _validationBlServiceMock.Setup(b => b.ValidateTransfer(from, to, sum))
                .Returns(new List<CustomModelError>());

            _currencyConverterMock.Setup(b => b.GetConvertedSum(sum, from.Currency, to.Currency))
                .Returns(10);

            // Act
            var (transaction, _) = _bankService.TryTransferMoney(_user, sum, from.CardNumber, to.CardNumber);

            // Assert
            _cardRepositoryMock.Verify(c => c.GetCard(_user, from.CardNumber), Times.Once);
            _cardRepositoryMock.Verify(c => c.GetCard(_user, to.CardNumber), Times.Once);
            _validationBlServiceMock.Verify(b => b.ValidateTransfer(from, to, sum), Times.Once);
            _currencyConverterMock.Verify(b => b.GetConvertedSum(sum, from.Currency, to.Currency),
                Times.Once);

            Assert.NotNull(transaction);
            Assert.Equal(sum, transaction.Sum);
            Assert.Equal(from.CardNumber, transaction.CardFromNumber);
            Assert.Equal(to.CardNumber, transaction.CardToNumber);
            Assert.Equal(fromBalance - sum, from.Balance);
            Assert.Equal(toBalance + sum, to.Balance);
        }

        [Fact]
        public void TryTransferMoney_EqualsCards_ReturnError()
        {
            // Arrange
            var from = _cards.First();
            var to = _cards.First();
            var fromBalance = from.Balance;
            var toBalance = to.Balance;
            const decimal sum = 10M;

            _validationBlServiceMock.Setup(b => b.ValidateTransfer(from, to, sum))
                .Returns(new List<CustomModelError>(1)
                {
                    new CustomModelError
                    {
                        Type = TypeCriticalException.CARD,
                        Message = "From card and to card is Equal",
                        FieldName = "from",
                        LocalizedMessage = "Нельзя перевести на ту же карту"
                    }
                });

            // Act
            var (_, errors) = _bankService.TryTransferMoney(_user, sum, from.CardNumber, to.CardNumber);

            // Assert
            _cardRepositoryMock.Verify(c => c.GetCard(_user, from.CardNumber), Times.AtMost(2));
            _validationBlServiceMock.Verify(b => b.ValidateTransfer(from, to, sum), Times.Once);
            _currencyConverterMock.Verify(b => b.GetConvertedSum(sum, from.Currency, to.Currency),
                Times.Never);

            Assert.Single(errors);
            Assert.Equal(TypeCriticalException.CARD, errors.First().Type);
            Assert.Equal("From card and to card is Equal", errors.First().Message);
            Assert.Equal("Нельзя перевести на ту же карту", errors.First().LocalizedMessage);
            Assert.Equal("from", errors.First().FieldName);
            Assert.Equal(fromBalance, from.Balance);
            Assert.Equal(toBalance, to.Balance);
        }

        [Fact]
        public void TryTransferMoney_EqualsCards_ReturnNullTransaction()
        {
            // Arrange
            var from = _cards.First();
            var to = _cards.First();
            const decimal sum = 10M;

            _validationBlServiceMock.Setup(b => b.ValidateTransfer(from, to, sum))
                .Returns(new List<CustomModelError>(1)
                {
                    new CustomModelError
                    {
                        Type = TypeCriticalException.CARD,
                        Message = "From card and to card is Equal",
                        FieldName = "from",
                        LocalizedMessage = "Нельзя перевести на ту же карту"
                    }
                });

            // Act
            var (transaction, _) = _bankService.TryTransferMoney(_user, sum, from.CardNumber, to.CardNumber);

            // Assert
            _cardRepositoryMock.Verify(c => c.GetCard(_user, from.CardNumber), Times.AtMost(2));
            _validationBlServiceMock.Verify(b => b.ValidateTransfer(from, to, sum), Times.Once);
            _currencyConverterMock.Verify(b => b.GetConvertedSum(sum, from.Currency, to.Currency),
                Times.Never);

            Assert.Null(transaction);
        }
    }
}