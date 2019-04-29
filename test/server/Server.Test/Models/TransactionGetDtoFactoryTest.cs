using AlfaBank.Core.Exceptions;
using AlfaBank.Core.Infrastructure;
using AlfaBank.Core.Models;
using AlfaBank.Core.Models.Dto;
using AlfaBank.Core.Models.Factories;
using Server.Test.Mocks;
using Server.Test.Mocks.Services;
using Server.Test.Utils;
using System.Diagnostics.CodeAnalysis;
using Xunit;

// ReSharper disable PossibleMultipleEnumeration
namespace Server.Test.Models
{
    [ExcludeFromCodeCoverage]
    public class TransactionGetDtoFactoryTest : DtoFactoryAbstractTest
    {
        private readonly TestDataGenerator _testDataGenerator;

        private readonly IDtoFactory<Transaction, TransactionGetDto> _dtoFactory;

        public TransactionGetDtoFactoryTest()
        {
            var cardNumberGenerator = new CardNumberGeneratorMockFactory().MockObject();
            var cardService = new CardServiceMockFactory().MockObject();
            _testDataGenerator = new TestDataGenerator(cardService, cardNumberGenerator);

            _dtoFactory = new TransactionGetDtoFactory(GetMapper());
        }

        [Fact]
        public void Map_ReturnValidDto()
        {
            // Arrange
            var fakeCard = _testDataGenerator.GenerateFakeCard(
                new CardPostDto
                {
                    Name = "my card",
                    Currency = (int)Currency.RUR,
                    Type = (int)CardType.MAESTRO
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
        public void Map_NullCard_ThrowException()
        {
            // Arrange
            var fakeCard = _testDataGenerator.GenerateFakeCard(
                new CardPostDto
                {
                    Name = "my card",
                    Currency = (int)Currency.RUR,
                    Type = (int)CardType.MAESTRO
                });
            var fakeTransaction = TestDataGenerator.GenerateFakeTransaction(fakeCard);
            fakeTransaction.Card = null;

            // Act
            Assert.Throws<CriticalException>(() => _dtoFactory.Map(fakeTransaction, _ => true));
        }

        [Fact]
        public void Map_ValidateFail_ReturnNull()
        {
            // Arrange
            var fakeCard = _testDataGenerator.GenerateFakeCard(
                new CardPostDto
                {
                    Name = "my card",
                    Currency = (int)Currency.RUR,
                    Type = (int)CardType.MAESTRO
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
            var fakeCard = _testDataGenerator.GenerateFakeCard(
                new CardPostDto
                {
                    Name = "my card",
                    Currency = (int)Currency.RUR,
                    Type = (int)CardType.MAESTRO
                });
            var fakeTransactions = TestDataGenerator.GenerateFakeTransactions(fakeCard);

            // Act
            var dtoList = _dtoFactory.Map(fakeTransactions, _ => true);

            // Assert
            Assert.NotEmpty(dtoList);
            Assert.All(
                dtoList,
                dto =>
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
            var fakeCard = _testDataGenerator.GenerateFakeCard(
                new CardPostDto
                {
                    Name = "my card",
                    Currency = (int)Currency.RUR,
                    Type = (int)CardType.MAESTRO
                });
            var fakeTransactions = TestDataGenerator.GenerateFakeTransactions(fakeCard);

            // Act
            var dtoList = _dtoFactory.Map(fakeTransactions, _ => false);

            // Assert
            Assert.Empty(dtoList);
        }
    }
}