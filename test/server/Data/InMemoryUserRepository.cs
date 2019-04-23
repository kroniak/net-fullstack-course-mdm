using Moq;
using Server.Data;
using Server.Data.Interfaces;
using ServerTest.Mocks;
using ServerTest.Mocks.Data;
using ServerTest.Mocks.Services;
using ServerTest.Utils;
using Xunit;

namespace ServerTest.Data
{
    public class InMemoryUserRepositoryTest
    {
        private readonly IUserRepository _userRepository;

        public InMemoryUserRepositoryTest()
        {
            var cardService = new CardServiceMockFactory().MockObject();
            var cardNumberGenerator = new CardNumberGeneratorMockFactory().MockObject();

            var testDataGenerator = new TestDataGenerator(cardService, cardNumberGenerator);
            var user = testDataGenerator.GenerateFakeUser();
            var cards = testDataGenerator.GenerateFakeCards();

            var fakeDataGeneratorMock = new Mock<IFakeDataGenerator>();
            fakeDataGeneratorMock.Setup(f => f.GenerateFakeUser()).Returns(user);
            fakeDataGeneratorMock.Setup(f => f.GenerateFakeCards()).Returns(cards);

            var cardRepositoryMock = new CardsRepositoryMockFactory(user).Mock();

            _userRepository = new InMemoryUserRepository(fakeDataGeneratorMock.Object, cardRepositoryMock.Object);
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