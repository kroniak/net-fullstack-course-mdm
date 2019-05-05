using AlfaBank.Core.Data;
using AlfaBank.Core.Data.Interfaces;
using AlfaBank.Services.Interfaces;
using Moq;
using Server.Test.Mocks;
using Server.Test.Mocks.Services;
using Server.Test.Utils;
using Xunit;

// ReSharper disable PossibleMultipleEnumeration
namespace Server.Test.Data
{
    public class InMemoryUserRepositoryTest
    {
        private readonly InMemoryUserRepository _userRepository;

        public InMemoryUserRepositoryTest()
        {
            var cardService = new CardServiceMockFactory().MockObject();
            var cardNumberGenerator = new CardNumberGeneratorMockFactory().MockObject();

            var testDataGenerator = new TestDataGenerator(cardService, cardNumberGenerator);
            var user = testDataGenerator.GenerateFakeUser();
            var cards = testDataGenerator.GenerateFakeCards();
            user.Cards.AddRange(cards);

            var fakeDataGeneratorMock = new Mock<IFakeDataGenerator>();
            fakeDataGeneratorMock.Setup(f => f.GenerateFakeUser()).Returns(user);
            fakeDataGeneratorMock.Setup(f => f.GenerateFakeCards()).Returns(cards);

            _userRepository = new InMemoryUserRepository(user);
        }

        [Fact]
        public void GetCurrentUser_ReturnCorrectUser()
        {
            // Act
            var user = _userRepository.GetCurrentUser();

            // Assert
            Assert.Equal("admin@admin.net", user.UserName);
        }
    }
}