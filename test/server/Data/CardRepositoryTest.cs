using System.Linq;
using Moq;
using Server.Data;
using Server.Data.Interfaces;
using Server.Models;
using Server.Services.Interfaces;
using ServerTest.Mocks;
using ServerTest.Mocks.Services;
using ServerTest.Utils;
using Xunit;

// ReSharper disable PossibleMultipleEnumeration
namespace ServerTest.Data
{
    public class CardRepositoryTest
    {
        private readonly Mock<ICardService> _cardServiceMock;
        private readonly TestDataGenerator _testDataGenerator;

        private readonly Card _card;
        private readonly User _user;

        private readonly ICardRepository _cardRepository;

        public CardRepositoryTest()
        {
            _cardServiceMock = new CardServiceMockFactory().Mock();
            var cardNumberGenerator = new CardNumberGeneratorMockFactory().MockObject();
            _testDataGenerator = new TestDataGenerator(_cardServiceMock.Object, cardNumberGenerator);

            _cardRepository = new CardRepository();

            var cards = _testDataGenerator.GenerateFakeCards();
            _card = cards.First();
            _user = TestDataGenerator.GenerateFakeUser(cards);
        }

        [Fact]
        public void GetCards_ReturnCorrectCardsList()
        {
            // Act
            var cards = _cardRepository.GetCards(_user);

            // Assert
            _cardServiceMock.Verify(x => x.TryAddBonusOnOpen(It.IsAny<Card>()), Times.AtMost(3));

            Assert.Equal(3, cards.Count);
            Assert.All(cards, c =>
                Assert.Single(c.Transactions)
            );
        }

        [Fact]
        public void GetCard_ExistCard_ReturnCorrectCard()
        {
            // Act
            var card = _cardRepository.GetCard(_user, _card.CardNumber);

            // Assert
            _cardServiceMock.Verify(x => x.TryAddBonusOnOpen(It.IsAny<Card>()), Times.AtMost(3));

            Assert.Single(card.Transactions);
            Assert.Equal(_card.ValidityYear, card.ValidityYear);
            Assert.Equal(_card.Currency, card.Currency);
            Assert.Equal(_card.CardNumber, card.CardNumber);
            Assert.Equal(_card.DtOpenCard, card.DtOpenCard);
            Assert.Equal(_card.CardType, card.CardType);
        }

        [Fact]
        public void GetCard_NotExistCard_ReturnNullCard()
        {
            // Arrange
            var cardDto = _testDataGenerator.GenerateFakeCard("4790878827491205");

            // Act
            var card = _cardRepository.GetCard(_user, cardDto.CardNumber);

            // Assert
            _cardServiceMock.Verify(x => x.TryAddBonusOnOpen(It.IsAny<Card>()), Times.AtMost(4));
            Assert.Null(card);
        }
    }
}