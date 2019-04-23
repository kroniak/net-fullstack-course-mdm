using System.Linq;
using Moq;
using Server.Data.Interfaces;
using Server.Models;

namespace ServerTest.Mocks.Data
{
    public class CardsRepositoryMockFactory : IMockFactory<ICardRepository>
    {
        private readonly Mock<ICardRepository> _mock;

        public CardsRepositoryMockFactory(User user)
        {
            _mock = new Mock<ICardRepository>();

            _mock.Setup(r => r.GetCards(user)).Returns(user.Cards);
            if (user.Cards.Any())
                _mock.Setup(r => r.GetCard(user, It.IsAny<string>())).Returns(user.Cards.First());
        }

        public Mock<ICardRepository> Mock() => _mock;

        public ICardRepository MockObject() => _mock.Object;
    }
}