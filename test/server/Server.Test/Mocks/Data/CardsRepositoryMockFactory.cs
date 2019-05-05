using AlfaBank.Core.Data.Interfaces;
using AlfaBank.Core.Models;
using Moq;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Server.Test.Mocks.Data
{
    [ExcludeFromCodeCoverage]
    public class CardsRepositoryMockFactory : IMockFactory<ICardRepository>
    {
        private readonly Mock<ICardRepository> _mock;

        public CardsRepositoryMockFactory(User user)
        {
            _mock = new Mock<ICardRepository>();

            _mock.Setup(r => r.GetAllWithTransactions(user)).Returns(user.Cards);

            if (!user.Cards.Any()) return;

            _mock.Setup(r => r.GetWithTransactions(user, It.IsAny<string>(), true)).Returns(user.Cards.First());
        }

        public Mock<ICardRepository> Mock() => _mock;

        public ICardRepository MockObject() => _mock.Object;
    }
}