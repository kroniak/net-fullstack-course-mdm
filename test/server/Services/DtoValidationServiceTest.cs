using System.Linq;
using Moq;
using Server.Models.Dto;
using Server.Services;
using Server.Services.Checkers;
using Server.Services.Interfaces;
using ServerTest.Mocks;
using ServerTest.Utils;
using Xunit;

namespace ServerTest.Services
{
    public class DtoValidationServiceTest
    {
        private readonly IDtoValidationService _dtoValidationService;
        private readonly Mock<ICardChecker> _cardCheckerMock;

        private const string WrongCardNumber = "4790878827491205123";
        private const string TrueCardNumber = "4790878827491205";

        public DtoValidationServiceTest()
        {
            _cardCheckerMock = new CardCheckerMockFactory().Mock();

            _dtoValidationService = new DtoValidationService(_cardCheckerMock.Object);
        }

        [Fact]
        public void ValidateOpenCardDto_InValidCard_Errors()
        {
            // Arrange
            var dto = TestDataGenerator.GenerateFakeValidityCardDto();

            // Act
            var errors = _dtoValidationService.ValidateOpenCardDto(dto);

            // Assert
            Assert.Equal(2, errors.Count());
            Assert.Equal(1, errors.Count(a => a.FieldName == "type"));
            Assert.Equal(1, errors.Count(a => a.FieldName == "currency"));
            Assert.Equal("Card type is invalid", errors.First(a => a.FieldName == "type").Message);
            Assert.Equal("Currency is invalid", errors.First(a => a.FieldName == "currency").Message);
        }

        [Fact]
        public void ValidateOpenCardDto_ValidCard_NoErrors()
        {
            // Arrange
            var dto = TestDataGenerator.GenerateCardDto();

            // Act
            var errors = _dtoValidationService.ValidateOpenCardDto(dto);

            // Assert
            Assert.Empty(errors);
        }

        [Fact]
        public void ValidateTransferDto_WrongFromCard_Error()
        {
            // Arrange
            var transaction = new TransactionPostDto
            {
                From = WrongCardNumber,
                To = TrueCardNumber,
                Sum = 1
            };

            // Act
            var errors = _dtoValidationService.ValidateTransferDto(transaction);

            // Assert
            _cardCheckerMock.Verify(x => x.CheckCardEmitter(WrongCardNumber), Times.Once);
            _cardCheckerMock.Verify(x => x.CheckCardEmitter(TrueCardNumber), Times.Once);

            Assert.Single(errors);
            Assert.Equal("from", errors.First().FieldName);
            Assert.Equal("Card number is invalid", errors.First().Message);
            Assert.Equal("Номер карты неверный", errors.First().LocalizedMessage);
        }

        [Fact]
        public void ValidateTransferDto_WrongToCard_Error()
        {
            // Arrange
            var transaction = new TransactionPostDto
            {
                From = TrueCardNumber,
                To = WrongCardNumber,
                Sum = 1
            };

            // Act
            var errors = _dtoValidationService.ValidateTransferDto(transaction);

            // Assert
            _cardCheckerMock.Verify(x => x.CheckCardEmitter(WrongCardNumber), Times.Once);
            _cardCheckerMock.Verify(x => x.CheckCardEmitter(TrueCardNumber), Times.Once);
            Assert.Single(errors);
            Assert.Equal("to", errors.First().FieldName);
            Assert.Equal("Card number is invalid", errors.First().Message);
            Assert.Equal("Номер карты неверный", errors.First().LocalizedMessage);
        }

        [Theory]
        [InlineData("4790878827491205", "5308276794485221", -1.0)]
        [InlineData("4790878827491205", "5308276794485221", 0)]
        public void ValidateTransferDto_InvalidSum_Error(string from, string to, decimal sum)
        {
            // Arrange
            var transaction = new TransactionPostDto
            {
                From = from,
                To = to,
                Sum = sum
            };

            // Act
            var errors = _dtoValidationService.ValidateTransferDto(transaction);

            // Assert
            _cardCheckerMock.Verify(x => x.CheckCardEmitter(from), Times.Once);
            _cardCheckerMock.Verify(x => x.CheckCardEmitter(to), Times.Once);
            Assert.Single(errors);
            Assert.Equal("sum", errors.First().FieldName);
            Assert.Equal("Sum must be greater then 0", errors.First().Message);
            Assert.Equal("Сумма должна быть больше 0", errors.First().LocalizedMessage);
        }

        [Fact]
        public void ValidateTransferDto_EqualsCard_Error()
        {
            // Arrange
            var transaction = new TransactionPostDto
            {
                From = TrueCardNumber,
                To = TrueCardNumber,
                Sum = 1
            };

            // Act
            var errors = _dtoValidationService.ValidateTransferDto(transaction);

            // Assert
            _cardCheckerMock.Verify(x => x.CheckCardEmitter(TrueCardNumber), Times.AtMost(2));
            Assert.Single(errors);
            Assert.Equal("from", errors.First().FieldName);
            Assert.Equal("From card and to card is Equal", errors.First().Message);
            Assert.Equal("Нельзя перевести на ту же карту", errors.First().LocalizedMessage);
        }

        [Theory]
        [InlineData("5308276794485221", "4790878827491205", 1)]
        public void ValidateTransferDto_ValidData_NoErrors(string from, string to, decimal sum)
        {
            // Arrange
            var transaction = new TransactionPostDto
            {
                From = from,
                To = to,
                Sum = sum
            };

            // Act
            var errors = _dtoValidationService.ValidateTransferDto(transaction);

            // Assert
            _cardCheckerMock.Verify(x => x.CheckCardEmitter(from), Times.Once);
            _cardCheckerMock.Verify(x => x.CheckCardEmitter(to), Times.Once);
            Assert.Empty(errors);
        }
    }
}