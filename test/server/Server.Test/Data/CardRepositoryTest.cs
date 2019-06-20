using System;
using AlfaBank.Core.Data;
using AlfaBank.Core.Data.Interfaces;
using AlfaBank.Core.Models;
using Server.Test.Mocks;
using Server.Test.Mocks.Services;
using Server.Test.Utils;
using System.Linq;
using AlfaBank.Core.Data.Repositories;
using AlfaBank.Core.Infrastructure;
using Xunit;

// ReSharper disable PossibleMultipleEnumeration
namespace Server.Test.Data
{
    public class CardRepositoryTest
    {
        private readonly TestDataGenerator _testDataGenerator;

        private readonly Card _card;
        private readonly User _user;
        private readonly SqlContext _context;

        private readonly ICardRepository _cardRepository;

        public CardRepositoryTest()
        {
            var cardServiceMock = new CardServiceMockFactory().Mock();
            var cardNumberGenerator = new CardNumberGeneratorMockFactory().MockObject();
            _testDataGenerator = new TestDataGenerator(cardServiceMock.Object, cardNumberGenerator);

            _context = SqlContextMock.GetSqlContext();

            _cardRepository = new CardRepository(_context);

            _card = _context.Cards.First();
            _user = _context.Users.FirstOrDefault(u => u.UserName == "admin@admin.ru");
        }

        [Fact]
        public void GetCards_ReturnCorrectCardsList()
        {
            // Act
            var cards = _cardRepository.GetAllWithTransactions(_user);

            // Assert
            Assert.Equal(4, cards.Count());
            Assert.False(_context.Tracked(cards));
            Assert.All(
                cards,
                c =>
                {
                    Assert.NotEqual(0, _card.Id);
                    Assert.NotNull(_card.User);
                    Assert.NotNull(_card.CardNumber);
                    Assert.NotEqual(0, _card.UserId);
                    Assert.NotNull(_card.CardName);
                    Assert.NotEqual(CardType.OTHER, _card.CardType);
                    Assert.Single(c.Transactions);
                });
        }

        [Fact]
        public void GetCard_ExistCard_ReturnCorrectCard()
        {
            // Act
            var card = _cardRepository.Get(_user, _card.CardNumber);

            Assert.Null(card.Transactions);
            Assert.True(_context.Tracked(card));
            Assert.Equal(_card.ValidityYear, card.ValidityYear);
            Assert.Equal(_card.Currency, card.Currency);
            Assert.Equal(_card.CardNumber, card.CardNumber);
            Assert.Equal(_card.DtOpenCard, card.DtOpenCard);
            Assert.Equal(_card.CardType, card.CardType);
        }

        [Fact]
        public void GetWithTransactions_ExistCard_ReturnCorrectCard()
        {
            // Act
            var card = _cardRepository.GetWithTransactions(_user, _card.CardNumber);

            Assert.Single(card.Transactions);
            Assert.True(_context.Tracked(card));
            Assert.Equal(_card.ValidityYear, card.ValidityYear);
            Assert.Equal(_card.Currency, card.Currency);
            Assert.Equal(_card.CardNumber, card.CardNumber);
            Assert.Equal(_card.DtOpenCard, card.DtOpenCard);
            Assert.Equal(_card.CardType, card.CardType);
        }

        [Fact]
        public void GetWithTransactions_NonExistCard_ReturnNullCard()
        {
            // Arrange
            var cardDto = _testDataGenerator.GenerateFakeCard(_user, "5315618148729302");

            // Act
            var card = _cardRepository.GetWithTransactions(_user, cardDto.CardNumber);

            // Assert
            Assert.Null(card);
        }

        [Fact]
        public void GetCard_NotExistCard_ReturnNullCard()
        {
            // Arrange
            var cardDto = _testDataGenerator.GenerateFakeCard(_user, "4790878827491205");

            // Act
            var card = _cardRepository.Get(_user, cardDto.CardNumber);

            // Assert
            Assert.Null(card);
        }

        [Fact]
        public void AddCard_NullCard_ThrowException() =>
            Assert.Throws<ArgumentNullException>(() => _cardRepository.Add(null));

        [Fact]
        public void AddCard_ExistCard_NoCardAdded()
        {
            // Arrange
            var expected = _context.Cards.Count();

            // Act
            _cardRepository.Add(_card);
            Assert.Throws<ArgumentException>(() => _cardRepository.Save());

            // Assert
            Assert.Equal(expected, _context.Cards.Count());
        }

        [Fact]
        public void AddCard_NewCard_CardAdded()
        {
            // Arrange
            var expected = _context.Cards.Count();
            var card = _testDataGenerator.GenerateFakeCard(_user, "4790878827491205", false);

            // Act
            _cardRepository.Add(card);
            _cardRepository.Save();

            // Assert
            Assert.Equal(expected + 1, _context.Cards.Count());
        }
    }
}