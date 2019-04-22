using Moq;
using Server.Services.Interfaces;

namespace ServerTest.Mocks.Services
{
    public class BusinessLogicValidationServiceMockFactory : IMockFactory<IBusinessLogicValidationService>
    {
        private readonly Mock<IBusinessLogicValidationService> _mock;

        public BusinessLogicValidationServiceMockFactory()
        {
            _mock = new Mock<IBusinessLogicValidationService>();
        }

        public Mock<IBusinessLogicValidationService> Mock() => _mock;

        public IBusinessLogicValidationService MockObject() => _mock.Object;
    }
}