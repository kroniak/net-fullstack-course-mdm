using AlfaBank.Core.Infrastructure;
using AlfaBank.Services.Interfaces;
using Moq;

namespace Server.Test.Mocks
{
    /// <inheritdoc />
    public class CardNumberGeneratorMockFactory : IMockFactory<ICardNumberGenerator>
    {
        private readonly Mock<ICardNumberGenerator> _mock;

        public CardNumberGeneratorMockFactory()
        {
            _mock = new Mock<ICardNumberGenerator>();

            _mock.Setup(r => r.GenerateNewCardNumber(CardType.MAESTRO)).Returns("6271190189011743");
            _mock.Setup(r => r.GenerateNewCardNumber(CardType.VISA)).Returns("4083966051329807");

            _mock.SetupSequence(r => r.GenerateNewCardNumber(CardType.MASTERCARD))
                .Returns("5101263135086131")
                .Returns("5308276794485221");
        }

        public Mock<ICardNumberGenerator> Mock() => _mock;

        public ICardNumberGenerator MockObject() => _mock.Object;
    }
}