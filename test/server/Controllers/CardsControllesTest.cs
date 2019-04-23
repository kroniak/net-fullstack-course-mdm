using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Server.Controllers;
using Server.Data.Interfaces;
using Server.Exceptions;
using Server.Infrastructure;
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
    public class CardsControllerTest : ControllerTestBase
    {
        private readonly TestDataGenerator _testDataGenerator;

        // Mocks and Fakes
        private readonly IEnumerable<Card> _fakeCards;
        private readonly IEnumerable<CardGetDto> _fakeCardsGetDtoList;
        private readonly User _user;

        private readonly Mock<ICardRepository> _cardRepositoryMock;
        private readonly Mock<ICardChecker> _cardCheckerMock;
        private readonly Mock<IBankService> _bankServiceMock;
        private readonly Mock<IDtoValidationService> _dtoValidationServiceMock;
        private readonly Mock<IDtoFactory<Card, CardGetDto>> _dtoFactoryMock;

        private readonly CardsController _controller;

        public CardsControllerTest()
        {
            _cardCheckerMock = new CardCheckerMockFactory().Mock();
            _dtoValidationServiceMock = new DtoValidationServiceMockFactory().Mock();
            _dtoFactoryMock = new Mock<IDtoFactory<Card, CardGetDto>>();
            var cardService = new CardServiceMockFactory().MockObject();
            var cardNumberGenerator = new CardNumberGeneratorMockFactory().MockObject();

            _testDataGenerator = new TestDataGenerator(cardService, cardNumberGenerator);
            _bankServiceMock = new Mock<IBankService>();

            // testData
            _fakeCards = _testDataGenerator.GenerateFakeCards();
            _fakeCardsGetDtoList = TestDataGenerator.GenerateFakeCardGetDtoList(_fakeCards);
            _user = TestDataGenerator.GenerateFakeUser(_fakeCards);
            var userRepositoryMock = new UserRepositoryMockFactory(_user).Mock();

            _cardRepositoryMock = new CardsRepositoryMockFactory(_user).Mock();

            var objectValidatorMock = GetMockObjectValidator();

            _controller = new CardsController(_dtoValidationServiceMock.Object,
                _cardRepositoryMock.Object,
                userRepositoryMock.Object,
                _cardCheckerMock.Object,
                _bankServiceMock.Object,
                _dtoFactoryMock.Object
            )
            {
                ObjectValidator = objectValidatorMock.Object
            };
        }

        #region GetCards

        [Fact]
        public void GetCards_ValidData_ReturnCorrectListResult()
        {
            var (result, _) = GetCards_ValidData();

            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public void GetCards_ValidData_ReturnOKResult()
        {
            var (_, cards) = GetCards_ValidData();

            Assert.Equal(_fakeCards.Count(), cards.Count());
        }

        private (OkObjectResult, IEnumerable<CardGetDto>) GetCards_ValidData()
        {
            // Arrange
            _dtoFactoryMock.Setup(d => d.Map(_fakeCards, It.IsAny<Func<CardGetDto, bool>>()))
                .Returns(_fakeCardsGetDtoList);

            // Act
            var result = (OkObjectResult) _controller.Get().Result;
            var cards = (IEnumerable<CardGetDto>) result.Value;

            // Assert
            _cardRepositoryMock.Verify(r => r.GetCards(_user), Times.Once);
            _dtoFactoryMock.Verify(d => d.Map(_fakeCards, It.IsAny<Func<CardGetDto, bool>>()), Times.Once());
            return (result, cards);
        }

        private (OkObjectResult, IEnumerable<CardGetDto>) GetCards_ValidDate_OutDtoValidationFail()
        {
            // Arrange
            _dtoFactoryMock.Setup(d => d.Map(_fakeCards, It.IsAny<Func<CardGetDto, bool>>()))
                .Returns(Enumerable.Empty<CardGetDto>());

            // Act
            var result = (OkObjectResult) _controller.Get().Result;
            var cards = (IEnumerable<CardGetDto>) result.Value;

            // Assert
            _cardRepositoryMock.Verify(r => r.GetCards(_user), Times.Once);
            _dtoFactoryMock.Verify(d => d.Map(_fakeCards, It.IsAny<Func<CardGetDto, bool>>()), Times.Once());
            return (result, cards);
        }

        [Fact]
        public void GetCards_ValidDate_OutDtoValidationFail_ReturnEmptyListResult()
        {
            var (_, cards) = GetCards_ValidDate_OutDtoValidationFail();

            Assert.Empty(cards);
        }

        [Fact]
        public void GetCards_ValidDate_OutDtoValidationFail_ReturnOkResult()
        {
            var (result, _) = GetCards_ValidDate_OutDtoValidationFail();

            Assert.Equal(200, result.StatusCode);
        }

        #endregion

        #region GetCard

        [Fact]
        public void GetCard_ValidData_OutDtoValidationFail_ReturnNotFoundResult()
        {
            // Arrange
            var fakeCard = GetCard_ValidData();
            _dtoFactoryMock.Setup(d => d.Map(fakeCard, It.IsAny<Func<CardGetDto, bool>>()))
                .Returns((CardGetDto) null);

            // Act
            var result = (NotFoundResult) _controller.Get(fakeCard.CardNumber).Result;

            // Assert
            _cardCheckerMock.Verify(r => r.CheckCardEmitter(fakeCard.CardNumber), Times.Once);
            _cardRepositoryMock.Verify(r => r.GetCard(_user, fakeCard.CardNumber), Times.Once);
            _dtoFactoryMock.Verify(d => d.Map(fakeCard, It.IsAny<Func<CardGetDto, bool>>()), Times.Once());

            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public void GetCard_ValidData_ReturnOKResult()
        {
            // Arrange
            var fakeCard = GetCard_ValidData();

            // Act
            var result = (OkObjectResult) _controller.Get(fakeCard.CardNumber).Result;

            // Assert
            _cardCheckerMock.Verify(r => r.CheckCardEmitter(fakeCard.CardNumber), Times.Once);
            _cardRepositoryMock.Verify(r => r.GetCard(_user, fakeCard.CardNumber), Times.Once);
            _dtoFactoryMock.Verify(d => d.Map(fakeCard, It.IsAny<Func<CardGetDto, bool>>()), Times.Once());

            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public void GetCard_ValidData_ReturnCorrectCard()
        {
            // Arrange
            var fakeCard = GetCard_ValidData();

            // Act
            var card = (CardGetDto) ((OkObjectResult) _controller.Get(fakeCard.CardNumber).Result).Value;

            // Assert
            _cardCheckerMock.Verify(r => r.CheckCardEmitter(fakeCard.CardNumber), Times.Once);
            _cardRepositoryMock.Verify(r => r.GetCard(_user, fakeCard.CardNumber), Times.Once);
            _dtoFactoryMock.Verify(d => d.Map(fakeCard, It.IsAny<Func<CardGetDto, bool>>()), Times.Once());

            Assert.Equal(fakeCard.CardName, card.Name);
            Assert.Equal(fakeCard.CardNumber, card.Number);
            Assert.Equal(3, card.Type);
            Assert.Equal(10M, card.Balance);
            Assert.Equal("01/22", card.Exp);
        }

        private Card GetCard_ValidData()
        {
            var fakeCard = _testDataGenerator.GenerateFakeCard(new CardPostDto
            {
                Name = "my card",
                Currency = (int) Currency.RUR,
                Type = (int) CardType.MAESTRO
            });

            var fakeCardGetDto = TestDataGenerator.GenerateFakeCardGetDto(fakeCard);

            _cardRepositoryMock.Setup(r => r.GetCard(_user, fakeCard.CardNumber)).Returns(fakeCard);
            _cardCheckerMock.Setup(r => r.CheckCardEmitter(fakeCard.CardNumber)).Returns(true);
            // Arrange
            _dtoFactoryMock.Setup(d => d.Map(fakeCard, It.IsAny<Func<CardGetDto, bool>>()))
                .Returns(fakeCardGetDto);

            return fakeCard;
        }

        [Theory]
        [InlineData("1234 1234 1233 1234")]
        [InlineData("12341233123")]
        [InlineData("5395029009021990")]
        [InlineData("4978588211036789")]
        public void GetCard_InvalidDto_ReturnBadRequest(string cardNumber)
        {
            // Act
            var getResult = _controller.Get(cardNumber);
            var result = (BadRequestObjectResult) getResult.Result;

            // Assert
            _cardCheckerMock.Verify(r => r.CheckCardEmitter(cardNumber), Times.Once);
            _cardRepositoryMock.Verify(r => r.GetCards(_user), Times.Never);

            Assert.IsType<BadRequestObjectResult>(getResult.Result);
            Assert.Equal(400, result.StatusCode);
            Assert.Null(getResult.Value);
        }

        #endregion

        #region PostCard

        [Fact]
        public void PostCard_ValidDto_ReturnOKResult()
        {
            // Arrange
            var cardDto = PostCard_ValidDto();

            // Act
            var result = (CreatedResult) _controller.Post(cardDto).Result;

            // Assert
            _dtoValidationServiceMock.Verify(v => v.ValidateOpenCardDto(cardDto), Times.Once);
            _bankServiceMock.Verify(
                v => v.TryOpenNewCard(_user, cardDto.Name, (Currency) cardDto.Currency, (CardType) cardDto.Type),
                Times.Once);
            _cardRepositoryMock.Verify(r => r.GetCards(_user), Times.Never);

            Assert.Equal(201, result.StatusCode);
        }

        [Fact]
        public void PostCard_ValidDto_ReturnCorrectOpenedCard()
        {
            // Arrange
            var cardDto = PostCard_ValidDto();

            // Act
            var resultCard = (CardGetDto) ((CreatedResult) _controller.Post(cardDto).Result).Value;

            // Assert
            _dtoValidationServiceMock.Verify(v => v.ValidateOpenCardDto(cardDto), Times.Once);
            _bankServiceMock.Verify(
                v => v.TryOpenNewCard(_user, cardDto.Name, (Currency) cardDto.Currency, (CardType) cardDto.Type),
                Times.Once);
            _cardRepositoryMock.Verify(r => r.GetCards(_user), Times.Never);

            Assert.Equal(10, resultCard.Balance);
            Assert.Equal(cardDto.Name, resultCard.Name);
            Assert.NotNull(resultCard.Number);
            Assert.Equal(cardDto.Currency, resultCard.Currency);
            Assert.Equal(cardDto.Type, resultCard.Type);
            Assert.Equal("01/22", resultCard.Exp);
        }

        private CardPostDto PostCard_ValidDto()
        {
            var cardDto = new CardPostDto
            {
                Name = "my card",
                Currency = 0,
                Type = 1
            };

            var fakeCard = _testDataGenerator.GenerateFakeCard(cardDto);
            var fakeCardGetDto = TestDataGenerator.GenerateFakeCardGetDto(fakeCard);

            _dtoValidationServiceMock
                .Setup(m => m.ValidateOpenCardDto(cardDto)).Returns(new List<CustomModelError>());

            _bankServiceMock
                .Setup(r => r.TryOpenNewCard(_user, cardDto.Name, (Currency) cardDto.Currency, (CardType) cardDto.Type))
                .Returns((fakeCard, new List<CustomModelError>()));

            _dtoFactoryMock.Setup(d => d.Map(fakeCard, It.IsAny<Func<CardGetDto, bool>>()))
                .Returns(fakeCardGetDto);

            return cardDto;
        }

        [Fact]
        public void PostCard_InternalError_ReturnBadRequest()
        {
            // Arrange
            var cardDto = new CardPostDto
            {
                Name = "my card",
                Currency = 0,
                Type = 1
            };

            _dtoValidationServiceMock
                .Setup(m => m.ValidateOpenCardDto(cardDto)).Returns(new List<CustomModelError>());

            _bankServiceMock
                .Setup(r => r.TryOpenNewCard(_user, cardDto.Name, (Currency) cardDto.Currency, (CardType) cardDto.Type))
                .Returns((null, new List<CustomModelError>
                {
                    new CustomModelError
                    {
                        FieldName = "internal",
                        Message = "Add bonus to card failed",
                        LocalizedMessage = "Ошибка при открытии карты"
                    }
                }));

            // Act
            var result = (BadRequestObjectResult) _controller.Post(cardDto).Result;

            // Assert
            _dtoValidationServiceMock.Verify(v => v.ValidateOpenCardDto(cardDto), Times.Once);
            _bankServiceMock.Verify(
                v => v.TryOpenNewCard(_user, cardDto.Name, (Currency) cardDto.Currency, (CardType) cardDto.Type),
                Times.Once);

            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void PostCard_EmptyName_ReturnBadRequest()
        {
            // Arrange
            var cardDto = new CardPostDto
            {
                Name = "",
                Currency = 0,
                Type = 1
            };

            var validationResultFake = new List<CustomModelError>
            {
                new CustomModelError
                {
                    FieldName = "name",
                    Message = ""
                }
            };

            PostCard_Field_ReturnBadRequest(cardDto, validationResultFake);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(5)]
        public void PostCard_WrongCurrency_ReturnBadRequest(int currency)
        {
            // Arrange
            var cardDto = new CardPostDto
            {
                Name = "name",
                Currency = currency,
                Type = 1
            };

            var validationResultFake = new List<CustomModelError>
            {
                new CustomModelError
                {
                    FieldName = "currency",
                    Message = ""
                }
            };

            PostCard_Field_ReturnBadRequest(cardDto, validationResultFake);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(5)]
        public void PostCard_WrongType_ReturnBadRequest(int type)
        {
            // Arrange
            var cardDto = new CardPostDto
            {
                Name = "name",
                Currency = 0,
                Type = type
            };

            var validationResultFake = new List<CustomModelError>
            {
                new CustomModelError
                {
                    FieldName = "currency",
                    Message = ""
                }
            };

            PostCard_Field_ReturnBadRequest(cardDto, validationResultFake);
        }

        private void PostCard_Field_ReturnBadRequest(CardPostDto cardDto,
            IEnumerable<CustomModelError> validationResultFake)
        {
            // Arrange
            _dtoValidationServiceMock.Setup(s => s.ValidateOpenCardDto(cardDto)).Returns(validationResultFake);

            // Act
            var result = (BadRequestObjectResult) _controller.Post(cardDto).Result;

            // Assert
            _dtoValidationServiceMock.Verify(v => v.ValidateOpenCardDto(cardDto), Times.Once);
            _bankServiceMock.Verify(
                v => v.TryOpenNewCard(_user, cardDto.Name, (Currency) cardDto.Currency, (CardType) cardDto.Type),
                Times.Never);

            Assert.Equal(400, result.StatusCode);
        }

        #endregion

        [Fact]
        public void PutCard_ReturnNotAllowed()
        {
            // Act
            var result = (StatusCodeResult) _controller.Put();

            // Assert
            Assert.Equal(405, result.StatusCode);
        }

        [Fact]
        public void DeleteCard_ReturnNotAllowed()
        {
            // Act
            var result = (StatusCodeResult) _controller.Delete();

            // Assert
            Assert.Equal(405, result.StatusCode);
        }
    }
}