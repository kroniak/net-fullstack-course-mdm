using AlfaBank.Core.Data;
using AlfaBank.Core.Data.Interfaces;
using AlfaBank.Core.Models;
using AlfaBank.Services.Interfaces;
using Moq;
using Server.Test.Mocks;
using Server.Test.Mocks.Services;
using Server.Test.Utils;
using System.Linq;
using Xunit;

// ReSharper disable PossibleMultipleEnumeration
namespace Server.Test.Data
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
            var cards = _cardRepository.All(_user);

            // Assert
            _cardServiceMock.Verify(x => x.TryAddBonusOnOpen(It.IsAny<Card>()), Times.Exactly(3));

            Assert.Equal(3, cards.Count());
            Assert.All(
                cards,
                c =>
                    Assert.Single(c.Transactions));
        }

        [Fact]
        public void GetCard_ExistCard_ReturnCorrectCard()
        {
            // Act
            var card = _cardRepository.Get(_user, _card.CardNumber);

            // Assert
            _cardServiceMock.Verify(x => x.TryAddBonusOnOpen(It.IsAny<Card>()), Times.Exactly(3));

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
            var card = _cardRepository.Get(_user, cardDto.CardNumber);

            // Assert
            _cardServiceMock.Verify(x => x.TryAddBonusOnOpen(It.IsAny<Card>()), Times.Exactly(4));
            Assert.Null(card);
        }

        [Fact]
        public void AddCard_NullCard_NoCardAdded()
        {
            // Arrange
            var expected = _user.Cards.Count;

            // Act
            _cardRepository.Add(_user, null);

            // Assert
            Assert.Equal(expected, _user.Cards.Count);
        }

        [Fact]
        public void AddCard_ExistCard_NoCardAdded()
        {
            // Arrange
            var expected = _user.Cards.Count;

            // Act
            _cardRepository.Add(_user, _card);

            // Assert
            Assert.Equal(expected, _user.Cards.Count);
        }

        [Fact]
        public void AddCard_NewCard_CardAdded()
        {
            // Arrange
            var expected = _user.Cards.Count;
            var card = _testDataGenerator.GenerateFakeCard("4790878827491205");

            // Act
            _cardRepository.Add(_user, card);

            // Assert
            Assert.Contains(_card, _user.Cards);
            Assert.Equal(expected + 1, _user.Cards.Count);
        }

        [Fact]
        public void RemoveCard_NullCard_NoCardRemoved()
        {
            // Arrange
            var expected = _user.Cards.Count;

            // Act
            _cardRepository.Remove(_user, null);

            // Assert
            Assert.Equal(expected, _user.Cards.Count);
        }

        [Fact]
        public void RemoveCard_ExistCard_CardRemoved()
        {
            // Arrange
            var expected = _user.Cards.Count;
            var card = _testDataGenerator.GenerateFakeCard(_card.CardNumber);
            
            // Act
            _cardRepository.Remove(_user, card);

            // Assert
            Assert.Equal(expected - 1, _user.Cards.Count);
        }

        [Fact]
        public void RemoveCard_NonExistCard_NoCardDeleted()
        {
            // Arrange
            var expected = _user.Cards.Count;
            var card = _testDataGenerator.GenerateFakeCard("4790878827491205");

            // Act
            _cardRepository.Remove(_user, card);

            // Assert
            Assert.Equal(expected, _user.Cards.Count);
        }
    }
}