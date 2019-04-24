using AlfaBank.Core.Infrastructure;
using AlfaBank.Core.Models;
using AlfaBank.Core.Models.Dto;
using AlfaBank.Core.Models.Factories;
using Server.Test.Mocks;
using Server.Test.Mocks.Services;
using Server.Test.Utils;
using Xunit;

// ReSharper disable PossibleMultipleEnumeration
namespace Server.Test.Models
{
    public class CardGetDtoFactoryTest : DtoFactoryAbstractTest
    {
        private readonly TestDataGenerator _testDataGenerator;

        private readonly IDtoFactory<Card, CardGetDto> _dtoFactory;

        public CardGetDtoFactoryTest()
        {
            var cardNumberGenerator = new CardNumberGeneratorMockFactory().MockObject();
            var cardService = new CardServiceMockFactory().MockObject();
            _testDataGenerator = new TestDataGenerator(cardService, cardNumberGenerator);

            _dtoFactory = new CardGetDtoFactory(GetMapper());
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

            // Act
            var dto = _dtoFactory.Map(fakeCard, _ => true);

            // Assert
            Assert.Equal(10M, dto.Balance);
            Assert.Equal(3, dto.Type);
            Assert.Equal(0, dto.Currency);
            Assert.Equal("01/22", dto.Exp);
            Assert.Equal("6271190189011743", dto.Number);
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

            // Act
            var dto = _dtoFactory.Map(fakeCard, _ => false);

            // Assert
            Assert.Null(dto);
        }

        [Fact]
        public void Map_ReturnValidDtoList()
        {
            // Arrange
            var fakeCards = _testDataGenerator.GenerateFakeCards();

            // Act
            var dtoList = _dtoFactory.Map(fakeCards, _ => true);

            // Assert
            Assert.NotEmpty(dtoList);
            Assert.All(dtoList, dto =>
            {
                Assert.Equal(10M, dto.Balance);
                Assert.InRange(dto.Type, 0, 4);
                Assert.InRange(dto.Currency, 0, 3);
                Assert.Equal("01/22", dto.Exp);
                Assert.Equal(16, dto.Number.Length);
            });
        }

        [Fact]
        public void Map_ValidateFail_ReturnEmptyList()
        {
            // Arrange
            var fakeCards = _testDataGenerator.GenerateFakeCards();

            // Act
            var dtoList = _dtoFactory.Map(fakeCards, _ => false);

            // Assert
            Assert.Empty(dtoList);
        }
    }
}