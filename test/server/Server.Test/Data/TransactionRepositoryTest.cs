using AlfaBank.Core.Data.Interfaces;
using AlfaBank.Core.Models;
using Server.Test.Mocks;
using Server.Test.Mocks.Services;
using Server.Test.Utils;
using System.Linq;
using AlfaBank.Core.Data.Repositories;
using Xunit;

// ReSharper disable PossibleMultipleEnumeration
namespace Server.Test.Data
{
    public class TransactionRepositoryTest
    {
        private readonly TestDataGenerator _testDataGenerator;

        private readonly ITransactionRepository _transactionsRepository;
        private readonly Card _card;
        private readonly User _user;

        public TransactionRepositoryTest()
        {
            var cardServiceMock = new CardServiceMockFactory().Mock();
            var cardNumberGenerator = new CardNumberGeneratorMockFactory().MockObject();
            _testDataGenerator = new TestDataGenerator(cardServiceMock.Object, cardNumberGenerator);

            var context = SqlContextMock.GetSqlContext();

            _transactionsRepository = new TransactionRepository(context);

            _card = context.Cards.First();
            _user = context.Users.FirstOrDefault(u => u.UserName == "admin@admin.ru");
        }

        [Fact]
        public void GetTransactions_ExistCard_ReturnCorrectTransactionsList()
        {
            // Act
            var transactions = _transactionsRepository.Get(_user, _card.CardNumber, 0, 10);

            // Assert
            Assert.Single(transactions);
            Assert.Null(transactions.First().CardFromNumber);
            Assert.Equal(_card.CardNumber, transactions.First().CardToNumber);
            Assert.Equal(10M, transactions.First().Sum);
        }

        [Fact]
        public void GetTransactions_NotExistCard_ReturnEmptyResult()
        {
            // Arrange
            var card = _testDataGenerator.GenerateFakeCard(_user, "4790878827491205");

            // Act
            var transactions = _transactionsRepository.Get(_user, card.CardNumber, 0, 10);

            Assert.Empty(transactions);
        }
    }
}