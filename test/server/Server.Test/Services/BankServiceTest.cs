﻿using AlfaBank.Core.Data.Interfaces;
using AlfaBank.Core.Exceptions;
using AlfaBank.Core.Infrastructure;
using AlfaBank.Core.Models;
using AlfaBank.Services;
using AlfaBank.Services.Converters;
using AlfaBank.Services.Interfaces;
using Moq;
using Server.Test.Mocks;
using Server.Test.Mocks.Data;
using Server.Test.Mocks.Services;
using Server.Test.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

// ReSharper disable PossibleMultipleEnumeration
// ReSharper disable ImplicitlyCapturedClosure
namespace Server.Test.Services
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
                _cardNumberGeneratorMock.Object);
        }

        [Theory]
        [InlineData(CardType.MASTERCARD)]
        [InlineData(CardType.VISA)]
        [InlineData(CardType.MAESTRO)]
        [InlineData(CardType.MIR)]
        public void TryOpenNewCard_CorrectTypeData_ReturnEmptyErrorsList(CardType type)
        {
            // Arrange
            _cardRepositoryMock.Setup(c => c.Add(It.IsAny<Card>()));
            _cardRepositoryMock.Setup(c => c.Get(_user, It.IsAny<string>())).Returns((Card) null);

            // Act
            var (_, errors) = _bankService.TryOpenNewCard(_user, "name", Currency.RUR, type);

            // Assert
            _cardRepositoryMock.Verify(c => c.Get(_user, It.IsAny<string>()), Times.Once);
            _cardNumberGeneratorMock.Verify(c => c.GenerateNewCardNumber(type), Times.Between(1, 2, Range.Inclusive));
            _cardRepositoryMock.Verify(c => c.Add(It.IsAny<Card>()), Times.Once);
            _cardServiceMock.Verify(x => x.TryAddBonusOnOpen(It.IsNotIn(_cards)), Times.Once);
            _cardRepositoryMock.Verify(c => c.Save(), Times.Once);

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
            _cardRepositoryMock.Setup(c => c.Add(It.IsAny<Card>()));
            _cardRepositoryMock.Setup(c => c.Get(_user, It.IsAny<string>())).Returns((Card) null);

            // Act
            var (card, _) = _bankService.TryOpenNewCard(_user, "name", Currency.RUR, type);

            // Assert
            _cardRepositoryMock.Verify(c => c.Get(_user, It.IsAny<string>()), Times.Once);
            _cardNumberGeneratorMock.Verify(c => c.GenerateNewCardNumber(type), Times.Between(1, 2, Range.Inclusive));
            _cardRepositoryMock.Verify(c => c.Add(It.IsAny<Card>()), Times.Once);
            _cardServiceMock.Verify(x => x.TryAddBonusOnOpen(It.IsNotIn(_cards)), Times.Once);
            _cardRepositoryMock.Verify(c => c.Save(), Times.Once);

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
            _cardRepositoryMock.Verify(c => c.GetAllWithTransactions(_user), Times.Never);
            _cardNumberGeneratorMock.Verify(c => c.GenerateNewCardNumber(CardType.OTHER), Times.Never);
            _cardServiceMock.Verify(x => x.TryAddBonusOnOpen(It.IsNotIn(_cards)), Times.Never);
            _cardRepositoryMock.Verify(c => c.Save(), Times.Never);

            Assert.Null(card);
        }

        [Fact]
        public void TryOpenNewCard_CardTypeOther_ReturnError()
        {
            // Act
            var (_, errors) = _bankService.TryOpenNewCard(_user, "name", Currency.RUR, CardType.OTHER);

            // Assert
            _cardRepositoryMock.Verify(c => c.GetAllWithTransactions(_user), Times.Never);
            _cardNumberGeneratorMock.Verify(c => c.GenerateNewCardNumber(CardType.OTHER), Times.Never);
            _cardServiceMock.Verify(x => x.TryAddBonusOnOpen(It.IsNotIn(_cards)), Times.Never);
            _cardRepositoryMock.Verify(c => c.Save(), Times.Never);

            Assert.Equal("type", errors.First().FieldName);
            Assert.Equal("Wrong type card", errors.First().Message);
            Assert.Equal("Неверный тип карты", errors.First().LocalizedMessage);
        }

        [Fact]
        public void TryOpenNewCard_ExistingCard_ReturnError()
        {
            // Assert
            var card = _cards.First();
            _cardRepositoryMock.Setup(c => c.Get(_user, It.IsAny<string>())).Returns(card);

            // Act
            var (_, errors) = _bankService.TryOpenNewCard(_user, "name", Currency.RUR, CardType.MASTERCARD);

            // Assert
            _cardRepositoryMock.Verify(c => c.Get(_user, It.IsAny<string>()), Times.Once);
            _cardNumberGeneratorMock.Verify(c => c.GenerateNewCardNumber(CardType.MASTERCARD), Times.Exactly(2));
            _cardRepositoryMock.Verify(c => c.Add(It.IsAny<Card>()), Times.Never);
            _cardServiceMock.Verify(x => x.TryAddBonusOnOpen(It.IsNotIn(_cards)), Times.Never);
            _cardRepositoryMock.Verify(c => c.Save(), Times.Never);

            Assert.NotEmpty(errors);
            Assert.Equal("internal", errors.First().FieldName);
            Assert.Equal("Card exist", errors.First().Message);
            Assert.Equal("Карта с таким номером уже существует", errors.First().LocalizedMessage);
        }

        [Fact]
        public void TryOpenNewCard_ExistingCard_ReturnNullCard()
        {
            // Assert
            var cardEx = _cards.First();
            _cardRepositoryMock.Setup(c => c.Get(_user, It.IsAny<string>())).Returns(cardEx);

            // Act
            var (card, _) = _bankService.TryOpenNewCard(_user, "name", Currency.RUR, CardType.MASTERCARD);

            // Assert
            _cardRepositoryMock.Verify(c => c.Get(_user, It.IsAny<string>()), Times.Once);
            _cardNumberGeneratorMock.Verify(c => c.GenerateNewCardNumber(CardType.MASTERCARD), Times.Exactly(2));
            _cardRepositoryMock.Verify(c => c.Add(It.IsAny<Card>()), Times.Never);
            _cardServiceMock.Verify(x => x.TryAddBonusOnOpen(It.IsNotIn(_cards)), Times.Never);
            _cardRepositoryMock.Verify(c => c.Save(), Times.Never);

            Assert.Null(card);
        }

        [Fact]
        public void TryOpenNewCard_TryAddBonusInternalError_ReturnError()
        {
            // Assert
            _cardRepositoryMock.Setup(c => c.Add(It.IsAny<Card>()));
            _cardRepositoryMock.Setup(c => c.Get(_user, It.IsAny<string>())).Returns((Card) null);
            _cardServiceMock.Setup(b => b.TryAddBonusOnOpen(It.IsAny<Card>())).Returns(false);

            // Act
            var (_, errors) = _bankService.TryOpenNewCard(_user, "name", Currency.RUR, CardType.MASTERCARD);

            // Assert
            _cardRepositoryMock.Verify(c => c.Get(_user, It.IsAny<string>()), Times.Once);
            _cardNumberGeneratorMock.Verify(c => c.GenerateNewCardNumber(CardType.MASTERCARD), Times.Exactly(2));
            _cardRepositoryMock.Verify(c => c.Add(It.IsAny<Card>()), Times.Once);
            _cardServiceMock.Verify(x => x.TryAddBonusOnOpen(It.IsNotIn(_cards)), Times.Once);
            _cardRepositoryMock.Verify(c => c.Save(), Times.Never);

            Assert.NotEmpty(errors);
            Assert.Equal("internal", errors.First().FieldName);
            Assert.Equal("Add bonus to card failed", errors.First().Message);
            Assert.Equal("Ошибка при открытии карты", errors.First().LocalizedMessage);
        }

        [Fact]
        public void TryOpenNewCard_TryAddBonusException_ReturnError()
        {
            // Assert
            _cardRepositoryMock.Setup(c => c.Add(It.IsAny<Card>()));
            _cardRepositoryMock.Setup(c => c.Get(_user, It.IsAny<string>())).Returns((Card) null);
            _cardServiceMock.Setup(b => b.TryAddBonusOnOpen(It.IsAny<Card>())).Throws<IOException>();

            // Act
            var (_, errors) = _bankService.TryOpenNewCard(_user, "name", Currency.RUR, CardType.MASTERCARD);

            // Assert
            _cardRepositoryMock.Verify(c => c.Get(_user, It.IsAny<string>()), Times.Once);
            _cardNumberGeneratorMock.Verify(c => c.GenerateNewCardNumber(CardType.MASTERCARD), Times.Exactly(2));
            _cardRepositoryMock.Verify(c => c.Add(It.IsAny<Card>()), Times.Once);
            _cardServiceMock.Verify(x => x.TryAddBonusOnOpen(It.IsNotIn(_cards)), Times.Once);
            _cardRepositoryMock.Verify(c => c.Save(), Times.Never);

            Assert.NotEmpty(errors);
            Assert.Equal("internal", errors.First().FieldName);
            Assert.Equal("I/O error occurred.", errors.First().Message);
            Assert.Equal("Что то пошло не так", errors.First().LocalizedMessage);
        }

        [Fact]
        public void TryOpenNewCard_TryAddBonusException_ReturnNullCard()
        {
            // Assert
            _cardRepositoryMock.Setup(c => c.Add(It.IsAny<Card>()));
            _cardRepositoryMock.Setup(c => c.Get(_user, It.IsAny<string>())).Returns((Card) null);
            _cardServiceMock.Setup(b => b.TryAddBonusOnOpen(It.IsAny<Card>())).Throws<IOException>();

            // Act
            var (card, _) = _bankService.TryOpenNewCard(_user, "name", Currency.RUR, CardType.MASTERCARD);

            // Assert
            _cardRepositoryMock.Verify(c => c.Get(_user, It.IsAny<string>()), Times.Once);
            _cardRepositoryMock.Verify(c => c.Add(It.IsAny<Card>()), Times.Once);
            _cardNumberGeneratorMock.Verify(c => c.GenerateNewCardNumber(CardType.MASTERCARD), Times.Exactly(2));
            _cardServiceMock.Verify(x => x.TryAddBonusOnOpen(It.IsNotIn(_cards)), Times.Once);
            _cardRepositoryMock.Verify(c => c.Save(), Times.Never);

            Assert.Null(card);
        }

        [Fact]
        public void TryOpenNewCard_DbSaveException_ReturnNullCard()
        {
            // Assert
            _cardRepositoryMock.Setup(c => c.Get(_user, It.IsAny<string>())).Returns((Card) null);
            _cardRepositoryMock.Setup(c => c.Add(It.IsAny<Card>()));
            _cardServiceMock.Setup(b => b.TryAddBonusOnOpen(It.IsAny<Card>())).Returns(true);
            _cardRepositoryMock.Setup(c => c.Save()).Throws<Exception>();

            // Act
            var (card, _) = _bankService.TryOpenNewCard(_user, "name", Currency.RUR, CardType.MASTERCARD);

            // Assert
            _cardRepositoryMock.Verify(c => c.Get(_user, It.IsAny<string>()), Times.Once);
            _cardRepositoryMock.Verify(c => c.Add(It.IsAny<Card>()), Times.Once);
            _cardNumberGeneratorMock.Verify(c => c.GenerateNewCardNumber(CardType.MASTERCARD), Times.Exactly(2));
            _cardServiceMock.Verify(x => x.TryAddBonusOnOpen(It.IsNotIn(_cards)), Times.Once);
            _cardRepositoryMock.Verify(c => c.Save(), Times.Once);

            Assert.Null(card);
        }

        [Fact]
        public void TryOpenNewCard_DbSaveException_ReturnErrorList()
        {
            // Assert
            _cardRepositoryMock.Setup(c => c.Get(_user, It.IsAny<string>())).Returns((Card) null);
            _cardRepositoryMock.Setup(c => c.Add(It.IsAny<Card>()));
            _cardServiceMock.Setup(b => b.TryAddBonusOnOpen(It.IsAny<Card>())).Returns(true);
            _cardRepositoryMock.Setup(c => c.Save()).Throws<Exception>();

            // Act
            var (_, errors) = _bankService.TryOpenNewCard(_user, "name", Currency.RUR, CardType.MASTERCARD);

            // Assert
            _cardRepositoryMock.Verify(c => c.Get(_user, It.IsAny<string>()), Times.Once);
            _cardRepositoryMock.Verify(c => c.Add(It.IsAny<Card>()), Times.Once);
            _cardNumberGeneratorMock.Verify(c => c.GenerateNewCardNumber(CardType.MASTERCARD), Times.Exactly(2));
            _cardServiceMock.Verify(x => x.TryAddBonusOnOpen(It.IsNotIn(_cards)), Times.Once);
            _cardRepositoryMock.Verify(c => c.Save(), Times.Once);

            Assert.NotEmpty(errors);
        }

        [Fact]
        public void TryTransferMoney_CorrectData_ReturnEmptyErrorsList()
        {
            // Arrange
            var from = _cards.First();
            var to = _cards.ElementAt(1);

            _cardRepositoryMock.Setup(c => c.GetWithTransactions(_user, from.CardNumber, false)).Returns(from);
            _cardRepositoryMock.Setup(c => c.GetWithTransactions(_user, to.CardNumber, false)).Returns(to);

            const decimal sum = 10M;

            _validationBlServiceMock.Setup(b => b.ValidateTransfer(from, to, sum))
                .Returns(Enumerable.Empty<CustomModelError>());

            _currencyConverterMock.Setup(b => b.GetConvertedSum(sum, from.Currency, to.Currency))
                .Returns(10);

            // Act
            var (_, errors) = _bankService.TryTransferMoney(_user, sum, from.CardNumber, to.CardNumber);

            // Assert
            _cardRepositoryMock.Verify(c => c.GetWithTransactions(_user, from.CardNumber, false), Times.Once);
            _cardRepositoryMock.Verify(c => c.GetWithTransactions(_user, to.CardNumber, false), Times.Once);
            _validationBlServiceMock.Verify(b => b.ValidateTransfer(from, to, sum), Times.Once);
            _currencyConverterMock.Verify(
                b => b.GetConvertedSum(sum, from.Currency, to.Currency),
                Times.Once);
            _cardRepositoryMock.Verify(c => c.Save(), Times.Once);

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

            _cardRepositoryMock.Setup(c => c.GetWithTransactions(_user, from.CardNumber, false)).Returns(from);
            _cardRepositoryMock.Setup(c => c.GetWithTransactions(_user, to.CardNumber, false)).Returns(to);

            const decimal sum = 10M;

            _validationBlServiceMock.Setup(b => b.ValidateTransfer(from, to, sum))
                .Returns(Enumerable.Empty<CustomModelError>());

            _currencyConverterMock.Setup(b => b.GetConvertedSum(sum, from.Currency, to.Currency))
                .Returns(10);

            // Act
            var (transaction, _) = _bankService.TryTransferMoney(_user, sum, from.CardNumber, to.CardNumber);

            // Assert
            _cardRepositoryMock.Verify(c => c.GetWithTransactions(_user, from.CardNumber, false), Times.Once);
            _cardRepositoryMock.Verify(c => c.GetWithTransactions(_user, to.CardNumber, false), Times.Once);
            _validationBlServiceMock.Verify(b => b.ValidateTransfer(from, to, sum), Times.Once);
            _currencyConverterMock.Verify(
                b => b.GetConvertedSum(sum, from.Currency, to.Currency),
                Times.Once);
            _cardRepositoryMock.Verify(c => c.Save(), Times.Once);

            Assert.NotNull(transaction);
            Assert.Equal(sum, transaction.Sum);
            Assert.Equal(from.CardNumber, transaction.CardFromNumber);
            Assert.Equal(to.CardNumber, transaction.CardToNumber);
            Assert.Equal(fromBalance - sum, from.Balance);
            Assert.Equal(toBalance + sum, to.Balance);
        }

        [Fact]
        public void TryTransferMoney_ExceptionOnAdd_ReturnSingleError()
        {
            // Arrange
            var from = _cards.First();
            var to = _cards.ElementAt(1);
            var fromBalance = from.Balance;
            var toBalance = to.Balance;

            _cardRepositoryMock.Setup(c => c.GetWithTransactions(_user, from.CardNumber, false)).Returns(from);
            _cardRepositoryMock.Setup(c => c.GetWithTransactions(_user, to.CardNumber, false)).Returns(to);

            const decimal sum = 10M;

            _validationBlServiceMock.Setup(b => b.ValidateTransfer(from, to, sum))
                .Returns(Enumerable.Empty<CustomModelError>());

            // exception
            _currencyConverterMock.Setup(b => b.GetConvertedSum(sum, from.Currency, to.Currency))
                .Throws<Exception>();

            // Act
            var (_, errors) =
                _bankService.TryTransferMoney(_user, sum, from.CardNumber, to.CardNumber);

            // Assert
            _cardRepositoryMock.Verify(c => c.GetWithTransactions(_user, from.CardNumber, false), Times.Once);
            _cardRepositoryMock.Verify(c => c.GetWithTransactions(_user, to.CardNumber, false), Times.Once);
            _validationBlServiceMock.Verify(b => b.ValidateTransfer(from, to, sum), Times.Once);
            _currencyConverterMock.Verify(
                b => b.GetConvertedSum(sum, from.Currency, to.Currency),
                Times.Once);
            _cardRepositoryMock.Verify(c => c.Save(), Times.Never);

            Assert.Single(errors);
            Assert.Equal(TypeCriticalException.TRANSACTION, errors.First().Type);
            Assert.Equal("Что то пошло не так", errors.First().LocalizedMessage);
            Assert.Equal("internal", errors.First().FieldName);
            Assert.Equal(fromBalance, from.Balance);
            Assert.Equal(toBalance, to.Balance);
        }

        [Fact]
        public void TryTransferMoney_ExceptionOnAdd_ReturnNullObject()
        {
            // Arrange
            var from = _cards.First();
            var to = _cards.ElementAt(1);

            _cardRepositoryMock.Setup(c => c.GetWithTransactions(_user, from.CardNumber, false)).Returns(from);
            _cardRepositoryMock.Setup(c => c.GetWithTransactions(_user, to.CardNumber, false)).Returns(to);

            const decimal sum = 10M;

            _validationBlServiceMock.Setup(b => b.ValidateTransfer(from, to, sum))
                .Returns(Enumerable.Empty<CustomModelError>());

            // exception
            _currencyConverterMock.Setup(b => b.GetConvertedSum(sum, from.Currency, to.Currency))
                .Throws<Exception>();

            // Act
            var (result, _) =
                _bankService.TryTransferMoney(_user, sum, from.CardNumber, to.CardNumber);

            // Assert
            _cardRepositoryMock.Verify(c => c.GetWithTransactions(_user, from.CardNumber, false), Times.Once);
            _cardRepositoryMock.Verify(c => c.GetWithTransactions(_user, to.CardNumber, false), Times.Once);
            _validationBlServiceMock.Verify(b => b.ValidateTransfer(from, to, sum), Times.Once);
            _currencyConverterMock.Verify(
                b => b.GetConvertedSum(sum, from.Currency, to.Currency),
                Times.Once);
            _cardRepositoryMock.Verify(c => c.Save(), Times.Never);

            Assert.Null(result);
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

            _cardRepositoryMock.Setup(c => c.GetWithTransactions(_user, from.CardNumber, false)).Returns(from);
            _cardRepositoryMock.Setup(c => c.GetWithTransactions(_user, to.CardNumber, false)).Returns(to);

            _validationBlServiceMock.Setup(b => b.ValidateTransfer(from, to, sum))
                .Returns(
                    new List<CustomModelError>
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
            _cardRepositoryMock.Verify(c => c.GetWithTransactions(_user, from.CardNumber, false), Times.Exactly(2));
            _validationBlServiceMock.Verify(b => b.ValidateTransfer(from, to, sum), Times.Once);
            _currencyConverterMock.Verify(
                b => b.GetConvertedSum(sum, from.Currency, to.Currency),
                Times.Never);
            _cardRepositoryMock.Verify(c => c.Save(), Times.Never);

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

            _cardRepositoryMock.Setup(c => c.GetWithTransactions(_user, from.CardNumber, false)).Returns(from);
            _cardRepositoryMock.Setup(c => c.GetWithTransactions(_user, to.CardNumber, false)).Returns(to);

            _validationBlServiceMock.Setup(b => b.ValidateTransfer(from, to, sum))
                .Returns(
                    new List<CustomModelError>
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
            _cardRepositoryMock.Verify(c => c.GetWithTransactions(_user, from.CardNumber, false), Times.Exactly(2));
            _validationBlServiceMock.Verify(b => b.ValidateTransfer(from, to, sum), Times.Once);
            _currencyConverterMock.Verify(
                b => b.GetConvertedSum(sum, from.Currency, to.Currency),
                Times.Never);
            _cardRepositoryMock.Verify(c => c.Save(), Times.Never);

            Assert.Null(transaction);
        }
    }
}