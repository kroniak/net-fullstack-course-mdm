using System.Diagnostics.CodeAnalysis;
using AlfaBank.Services.Interfaces;
using Moq;

namespace Server.Test.Mocks.Services
{
    [ExcludeFromCodeCoverage]
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