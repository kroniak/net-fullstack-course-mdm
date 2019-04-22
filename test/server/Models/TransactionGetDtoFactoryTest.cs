using Server.Infrastructure;
using Server.Models;
using Server.Models.Dto;
using Server.Models.Factories;
using ServerTest.Mocks;
using ServerTest.Mocks.Services;
using ServerTest.Utils;
using Xunit;

// ReSharper disable PossibleMultipleEnumeration
namespace ServerTest.Models
{
    public class TransactionGetDtoFactoryTest
    {
        private readonly TestDataGenerator _testDataGenerator;

        private readonly IDtoFactory<Transaction, TransactionGetDto> _dtoFactory = new TransactionGetDtoFactory();

        public TransactionGetDtoFactoryTest()
        {
            var cardNumberGenerator = new CardNumberGeneratorMockFactory().MockObject();
            var cardService = new CardServiceMockFactory().MockObject();
            _testDataGenerator = new TestDataGenerator(cardService, cardNumberGenerator);
        }

        [Fact]
        public void Map_ReturnValidDto()
        {
            // Arrange
            var fakeCard = _testDataGenerator.GenerateFakeCard(new CardPostDto
            {
                Name = "my card",
                Currency = (int) Currency.RUR,
                Type = (int) CardType.MAESTRO
            });
            var fakeTransaction = TestDataGenerator.GenerateFakeTransaction(fakeCard);

            // Act
            var dto = _dtoFactory.Map(fakeTransaction, _ => true);

            // Assert
            Assert.Equal(10M, dto.Sum);
            Assert.Equal("4083XXXXXXXX6239", dto.To);
            Assert.Equal(fakeCard.CardNumber, dto.From);
            Assert.False(dto.IsCredit);
        }

        [Fact]
        public void Map_ValidateFail_ReturnNull()
        {
            // Arrange
            var fakeCard = _testDataGenerator.GenerateFakeCard(new CardPostDto
            {
                Name = "my card",
                Currency = (int) Currency.RUR,
                Type = (int) CardType.MAESTRO
            });
            var fakeTransaction = TestDataGenerator.GenerateFakeTransaction(fakeCard);

            // Act
            var dto = _dtoFactory.Map(fakeTransaction, _ => false);

            // Assert
            Assert.Null(dto);
        }

        [Fact]
        public void Map_ReturnValidDtoList()
        {
            // Arrange
            var fakeCard = _testDataGenerator.GenerateFakeCard(new CardPostDto
            {
                Name = "my card",
                Currency = (int) Currency.RUR,
                Type = (int) CardType.MAESTRO
            });
            var fakeTransactions = TestDataGenerator.GenerateFakeTransactions(fakeCard);

            // Act
            var dtoList = _dtoFactory.Map(fakeTransactions, _ => true);

            // Assert
            Assert.NotEmpty(dtoList);
            Assert.All(dtoList, dto =>
            {
                Assert.Equal(10M, dto.Sum);
                Assert.Equal("4083XXXXXXXX6239", dto.To);
                Assert.Equal(fakeCard.CardNumber, dto.From);
                Assert.False(dto.IsCredit);
            });
        }

        [Fact]
        public void Map_ValidateFail_ReturnEmptyList()
        {
            // Arrange
            var fakeCard = _testDataGenerator.GenerateFakeCard(new CardPostDto
            {
                Name = "my card",
                Currency = (int) Currency.RUR,
                Type = (int) CardType.MAESTRO
            });
            var fakeTransactions = TestDataGenerator.GenerateFakeTransactions(fakeCard);

            // Act
            var dtoList = _dtoFactory.Map(fakeTransactions, _ => false);

            // Assert
            Assert.Empty(dtoList);
        }
    }
}