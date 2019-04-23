using System;
using Moq;
using Server.Services.Checkers;

namespace ServerTest.Mocks
{
    public class CardCheckerMockFactory : IMockFactory<ICardChecker>
    {
        private readonly Mock<ICardChecker> _mock;

        public CardCheckerMockFactory()
        {
            _mock = new Mock<ICardChecker>();

            string[] trueNumbers =
            {
                "4083967629457310",
                "5395 0290 0902 1990",
                "   4978 588211036789    ",
                "4083969259636239",
                "5101265622568232",
                "4790878827491205",
                "5308276794485221",
                "6271190189011743",
                "4083966051329807",
                "6762302693240520",
                "2203572242903770"
            };

            string[] falseNumbers =
            {
                "1234 1234 1233 1234",
                "1234123412331234",
                "",
                null,
                "5395029009021990",
                "4978588211036789",
                "4790878827491205123"
            };

            // Setup true result
            _mock.Setup(
                    r => r.CheckCardEmitter(It.Is<string>(n => Array.IndexOf(trueNumbers, n) > 0)))
                .Returns(true);

            // Setup failed result
            _mock.Setup(
                r => r.CheckCardEmitter(
                    It.Is<string>(n => Array.IndexOf(falseNumbers, n) > 0))).Returns(false);

            // Setup true result
            _mock.Setup(
                    r => r.CheckCardNumber(It.Is<string>(n => Array.IndexOf(trueNumbers, n) > 0)))
                .Returns(true);

            // Setup failed result
            _mock.Setup(
                r => r.CheckCardNumber(
                    It.Is<string>(n => Array.IndexOf(falseNumbers, n) > 0))).Returns(false);
        }

        public Mock<ICardChecker> Mock() => _mock;

        public ICardChecker MockObject() => _mock.Object;
    }
}