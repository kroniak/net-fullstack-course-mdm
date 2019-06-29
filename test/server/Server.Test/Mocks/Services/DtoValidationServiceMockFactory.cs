using AlfaBank.Services.Interfaces;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace Server.Test.Mocks.Services
{
    [ExcludeFromCodeCoverage]
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