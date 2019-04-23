using Moq;
using Server.Services.Interfaces;

namespace ServerTest.Mocks.Services
{
    public class DtoValidationServiceMockFactory : IMockFactory<IDtoValidationService>
    {
        private readonly Mock<IDtoValidationService> _mock;

        public DtoValidationServiceMockFactory()
        {
            _mock = new Mock<IDtoValidationService>();

            // Setup true result
        }

        public Mock<IDtoValidationService> Mock() => _mock;

        public IDtoValidationService MockObject() => _mock.Object;
    }
}