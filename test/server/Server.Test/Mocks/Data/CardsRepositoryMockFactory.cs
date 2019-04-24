using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AlfaBank.Core.Data.Interfaces;
using AlfaBank.Core.Models;
using Moq;

namespace Server.Test.Mocks.Data
{
    [ExcludeFromCodeCoverage]
    public class CardsRepositoryMockFactory : IMockFactory<ICardRepository>
    {
        private readonly Mock<ICardRepository> _mock;

        public CardsRepositoryMockFactory(User user)
        {
            _mock = new Mock<ICardRepository>();

            _mock.Setup(r => r.All(user)).Returns(user.Cards);
            if (user.Cards.Any())
                _mock.Setup(r => r.Get(user, It.IsAny<string>())).Returns(user.Cards.First());
        }

        public Mock<ICardRepository> Mock() => _mock;

        public ICardRepository MockObject() => _mock.Object;
    }
}