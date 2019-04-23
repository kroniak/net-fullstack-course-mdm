using Moq;
using Server.Models;
using Server.Services.Interfaces;

namespace ServerTest.Mocks.Services
{
    public class CardServiceMockFactory : IMockFactory<ICardService>
    {
        private readonly Mock<ICardService> _mock;

        public CardServiceMockFactory()
        {
            _mock = new Mock<ICardService>();

            // Setup true result
            _mock.Setup(x => x.TryAddBonusOnOpen(It.IsAny<Card>()))
                .Returns(true)
                .Callback<Card>(card =>
                {
                    card.Transactions.Add(new Transaction
                    {
                        Card = card,
                        CardToNumber = card.CardNumber,
                        Sum = 10M
                    });
                });
        }

        public Mock<ICardService> Mock() => _mock;

        public ICardService MockObject() => _mock.Object;
    }
}