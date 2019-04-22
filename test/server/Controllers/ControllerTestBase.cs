using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Moq;

namespace ServerTest.Controllers
{
    public abstract class ControllerTestBase
    {
        protected static Mock<IObjectModelValidator> GetMockObjectValidator()
        {
            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                It.IsAny<ValidationStateDictionary>(),
                It.IsAny<string>(),
                It.IsAny<object>()));

            return objectValidator;
        }
    }
}