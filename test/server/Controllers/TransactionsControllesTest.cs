using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Server.Controllers;
using Server.Data.Interfaces;
using Server.Exceptions;
using Server.Models;
using Server.Models.Dto;
using Server.Models.Factories;
using Server.Services.Checkers;
using Server.Services.Interfaces;
using ServerTest.Mocks;
using ServerTest.Mocks.Data;
using ServerTest.Mocks.Services;
using ServerTest.Utils;
using Xunit;

// ReSharper disable PossibleMultipleEnumeration

// ReSharper disable ImplicitlyCapturedClosure
namespace ServerTest.Controllers
{
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public class TransactionsControllerTest : ControllerTestBase
    {
        private readonly TestDataGenerator _testDataGenerator;

        // Mocks and Fakes
        private readonly List<Card> _fakeCards;
        private readonly User _user;

        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly Mock<ICardChecker> _cardCheckerMock;
        private readonly Mock<IBankService> _bankServiceMock;
        private readonly Mock<IDtoValidationService> _dtoValidationServiceMock;
        private readonly Mock<IDtoFactory<Transaction, TransactionGetDto>> _dtoFactoryMock;

        private readonly TransactionsController _controller;

        public TransactionsControllerTest()
        {
            _cardCheckerMock = new CardCheckerMockFactory().Mock();
            _dtoValidationServiceMock = new DtoValidationServiceMockFactory().Mock();
            _dtoFactoryMock = new Mock<IDtoFactory<Transaction, TransactionGetDto>>();

            var cardService = new CardServiceMockFactory().MockObject();
            var cardNumberGenerator = new CardNumberGeneratorMockFactory().MockObject();

            _testDataGenerator = new TestDataGenerator(cardService, cardNumberGenerator);
            _bankServiceMock = new Mock<IBankService>();
            _transactionRepositoryMock = new Mock<ITransactionRepository>();

            // testData
            _fakeCards = _testDataGenerator.GenerateFakeCards().ToList();
            _user = TestDataGenerator.GenerateFakeUser(_fakeCards);
            var userRepositoryMock = new UserRepositoryMockFactory(_user).Mock();

            var objectValidatorMock = GetMockObjectValidator();

            _controller = new TransactionsController(
                _dtoValidationServiceMock.Object,
                userRepositoryMock.Object,
                _transactionRepositoryMock.Object,
                _cardCheckerMock.Object,
                _bankServiceMock.Object,
                _dtoFactoryMock.Object
            )
            {
                ObjectValidator = objectValidatorMock.Object
            };
        }

        [Theory]
        [InlineData("1234 1234 1233 1234", 0)]
        [InlineData("12341233123", 0)]
        [InlineData("", 0)]
        [InlineData(null, 0)]
        [InlineData("5395029009021990", 0)]
        [InlineData("4978588211036789", 0)]
        public void GetTransactions_InvalidCardNumber_ReturnBadRequest(string value, int skip)
        {
            // Act
            var getResult = _controller.Get(value, skip);
            var result = (BadRequestObjectResult) getResult.Result;

            // Assert
            _transactionRepositoryMock.Verify(r => r.GetTransactions(_user, value, skip, 10), Times.Never);
            _cardCheckerMock.Verify(r => r.CheckCardEmitter(value), Times.AtMost(1));

            Assert.IsType<BadRequestObjectResult>(getResult.Result);
            Assert.Equal(400, result.StatusCode);
        }

        [Theory]
        [InlineData("4083969259636239", -1)]
        [InlineData("5101265622568232", -1)]
        public void GetTransactions_InvalidSkip_ReturnBadRequest(string value, int skip)
        {
            // Arrange
            _controller.ModelState.AddModelError("skip", "Skip must be greater than -1");

            // Act
            var getResult = _controller.Get(value, skip);
            var result = (BadRequestObjectResult) getResult.Result;

            // Assert
            _transactionRepositoryMock.Verify(r => r.GetTransactions(_user, value, skip, 10), Times.Never);
            _cardCheckerMock.Verify(r => r.CheckCardEmitter(value), Times.AtMost(1));

            Assert.IsType<BadRequestObjectResult>(getResult.Result);
            Assert.Equal(400, result.StatusCode);
        }

        [Theory]
        [InlineData("4083969259636239")]
        [InlineData("5101265622568232")]
        public void GetTransactions_ValidData_OutDtoValidationFail_ReturnEmptyListResult(string value)
        {
            var (_, transactions) = GetTransactions_ValidData_OutDtoValidationFail(value);

            Assert.Empty(transactions);
        }

        [Theory]
        [InlineData("4083969259636239")]
        [InlineData("5101265622568232")]
        public void GetTransactions_ValidData_OutDtoValidationFail_ReturnOkResult(string value)
        {
            var (result, _) = GetTransactions_ValidData_OutDtoValidationFail(value);

            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Theory]
        [InlineData("5101265622568232")]
        public void GetTransactions_ValidData_ReturnCorrectListResult(string value)
        {
            var (_, transactions) = GetTransactions_ValidData(value);

            Assert.Equal(5, transactions.Count());
            Assert.All(transactions, dto =>
                {
                    Assert.False(dto.IsCredit);
                    Assert.Equal(10M, dto.Sum);
                    Assert.Equal("4083XXXXXXXX6239", dto.To);
                    Assert.Equal(value, dto.From);
                }
            );
        }

        [Theory]
        [InlineData("4083969259636239")]
        [InlineData("5101265622568232")]
        public void GetTransactions_ValidData_ReturnOKResult(string value)
        {
            var (result, _) = GetTransactions_ValidData(value);

            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, result.StatusCode);
        }

        private (OkObjectResult result, IEnumerable<TransactionGetDto> transactions)
            GetTransactions_ValidData_OutDtoValidationFail(
                string value)
        {
            // Arrange
            var fakeCard = _testDataGenerator.GenerateFakeCard(value);
            var fakeTransactions = TestDataGenerator.GenerateFakeTransactions(fakeCard);

            _transactionRepositoryMock.Setup(r => r.GetTransactions(_user, value, 0, 10))
                .Returns(fakeTransactions);
            _dtoFactoryMock.Setup(d => d.Map(fakeTransactions, It.IsAny<Func<TransactionGetDto, bool>>()))
                .Returns(Enumerable.Empty<TransactionGetDto>());

            // Act
            var result = (OkObjectResult) _controller.Get(value).Result;
            var transactions = (IEnumerable<TransactionGetDto>) result.Value;

            // Assert
            _cardCheckerMock.Verify(r => r.CheckCardEmitter(value), Times.Once);
            _transactionRepositoryMock.Verify(r => r.GetTransactions(_user, value, 0, 10), Times.Once);
            _dtoFactoryMock.Verify(d => d.Map(fakeTransactions, It.IsAny<Func<TransactionGetDto, bool>>()),
                Times.Once());
            return (result, transactions);
        }

        private (OkObjectResult result, IEnumerable<TransactionGetDto> transactions) GetTransactions_ValidData(
            string value)
        {
            // Arrange
            var fakeCard = _testDataGenerator.GenerateFakeCard(value);
            var fakeTransactions = TestDataGenerator.GenerateFakeTransactions(fakeCard);
            var fakeGetTransactionDtoList = TestDataGenerator.GenerateFakeGetTransactionDtoList(fakeTransactions);

            _transactionRepositoryMock.Setup(r => r.GetTransactions(_user, value, 0, 10))
                .Returns(fakeTransactions);
            _dtoFactoryMock.Setup(d => d.Map(fakeTransactions, It.IsAny<Func<TransactionGetDto, bool>>()))
                .Returns(fakeGetTransactionDtoList);

            // Act
            var result = (OkObjectResult) _controller.Get(value).Result;
            var transactions = (IEnumerable<TransactionGetDto>) result.Value;

            // Assert
            _cardCheckerMock.Verify(r => r.CheckCardEmitter(value), Times.Once);
            _transactionRepositoryMock.Verify(r => r.GetTransactions(_user, value, 0, 10), Times.Once);
            _dtoFactoryMock.Verify(d => d.Map(fakeTransactions, It.IsAny<Func<TransactionGetDto, bool>>()),
                Times.Once());
            return (result, transactions);
        }

        [Fact]
        public void PostTransactions_ValidData_ReturnCreatedResult()
        {
            // Arrange
            var fakeCardFrom = _fakeCards.First();
            var fakeCardTo = _fakeCards.ElementAt(1);

            var fakePostTransactionDto = TestDataGenerator.GenerateFakePostTransactionDto(fakeCardFrom, fakeCardTo);
            var fakeTransaction = TestDataGenerator.GenerateFakeTransaction(fakeCardFrom, fakePostTransactionDto);
            var fakeGetTransactionDto = TestDataGenerator.GenerateFakeGetTransactionDto(fakeTransaction);

            _dtoValidationServiceMock.Setup(d => d.ValidateTransferDto(fakePostTransactionDto))
                .Returns(new List<CustomModelError>());
            _bankServiceMock.Setup(r => r.TryTransferMoney(_user, fakePostTransactionDto.Sum,
                    fakePostTransactionDto.From,
                    fakePostTransactionDto.To))
                .Returns((fakeTransaction, new List<CustomModelError>()));
            _dtoFactoryMock.Setup(d => d.Map(fakeTransaction, It.IsAny<Func<TransactionGetDto, bool>>()))
                .Returns(fakeGetTransactionDto);

            // Act
            var result = (CreatedResult) _controller.Post(fakePostTransactionDto).Result;
            var transaction = (TransactionGetDto) result.Value;

            // Assert
            _dtoValidationServiceMock.Verify(d => d.ValidateTransferDto(fakePostTransactionDto), Times.Once);
            _bankServiceMock.Verify(r => r.TryTransferMoney(_user, fakePostTransactionDto.Sum,
                fakePostTransactionDto.From,
                fakePostTransactionDto.To), Times.Once);
            _dtoFactoryMock.Verify(d => d.Map(fakeTransaction, It.IsAny<Func<TransactionGetDto, bool>>()), Times.Once);

            Assert.Equal(fakePostTransactionDto.From, transaction.From);
            Assert.Equal(fakePostTransactionDto.To.Substring(0, 4), transaction.To.Substring(0, 4));
            Assert.Equal(fakePostTransactionDto.To.Substring(fakePostTransactionDto.To.Length - 4, 4),
                transaction.To.Substring(transaction.To.Length - 4, 4));
            Assert.Equal("XXXXXXXX", transaction.To.Substring(4, 8));
            Assert.Equal(fakePostTransactionDto.Sum, transaction.Sum);
            Assert.False(transaction.IsCredit);
        }

        [Fact]
        public void PostTransactions_InternalError_ReturnBadRequest()
        {
            // Arrange
            var cardFrom = _fakeCards.First();
            var cardTo = _fakeCards.ElementAt(1);

            var fakeTransactionDto = TestDataGenerator.GenerateFakePostTransactionDto(cardFrom, cardTo);

            _dtoValidationServiceMock.Setup(d => d.ValidateTransferDto(fakeTransactionDto))
                .Returns(new List<CustomModelError>());
            _bankServiceMock.Setup(r => r.TryTransferMoney(_user, fakeTransactionDto.Sum, fakeTransactionDto.From,
                    fakeTransactionDto.To))
                .Returns((null, new List<CustomModelError>
                {
                    new CustomModelError
                    {
                        FieldName = "internal",
                        Message = "Internal error",
                        LocalizedMessage = "Что то пошло не так"
                    }
                }));

            // Act
            var postResult = _controller.Post(fakeTransactionDto);
            var result = (BadRequestObjectResult) postResult.Result;

            // Assert
            _dtoValidationServiceMock.Verify(d => d.ValidateTransferDto(fakeTransactionDto), Times.Once);
            _bankServiceMock.Verify(r => r.TryTransferMoney(_user, fakeTransactionDto.Sum, fakeTransactionDto.From,
                fakeTransactionDto.To), Times.Once);

            Assert.IsType<BadRequestObjectResult>(postResult.Result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void PostTransactions_InvalidDTO_ReturnBadRequest()
        {
            // Arrange
            var cardFrom = _fakeCards.First();
            var cardTo = _fakeCards.First();

            var fakeTransactionDto = TestDataGenerator.GenerateFakePostTransactionDto(cardFrom, cardTo);

            _dtoValidationServiceMock.Setup(d => d.ValidateTransferDto(fakeTransactionDto))
                .Returns(new List<CustomModelError>
                {
                    new CustomModelError
                    {
                        FieldName = "from",
                        LocalizedMessage = "Нельзя перевести на ту же карту",
                        Message = "From card and to card is Equal"
                    }
                });

            // Act
            var postResult = _controller.Post(fakeTransactionDto);
            var result = (BadRequestObjectResult) postResult.Result;

            // Assert
            _dtoValidationServiceMock.Verify(d => d.ValidateTransferDto(fakeTransactionDto), Times.Once);
            _bankServiceMock.Verify(r => r.TryTransferMoney(_user, fakeTransactionDto.Sum, fakeTransactionDto.From,
                fakeTransactionDto.To), Times.Never);

            Assert.IsType<BadRequestObjectResult>(postResult.Result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void PutTransaction_ReturnMethodNotAllowed()
        {
            var result = (StatusCodeResult) _controller.Put();

            Assert.Equal(405, result.StatusCode);
        }

        [Fact]
        public void DeleteTransaction_ReturnMethodNotAllowed()
        {
            var result = (StatusCodeResult) _controller.Delete();

            Assert.Equal(405, result.StatusCode);
        }
    }
}