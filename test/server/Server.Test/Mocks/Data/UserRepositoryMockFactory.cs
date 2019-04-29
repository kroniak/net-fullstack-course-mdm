using AlfaBank.Core.Data.Interfaces;
using AlfaBank.Core.Models;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace Server.Test.Mocks.Data
{
    [ExcludeFromCodeCoverage]
    public class UserRepositoryMockFactory : IMockFactory<IUserRepository>
    {
        private readonly Mock<IUserRepository> _mock;

        public UserRepositoryMockFactory(User user)
        {
            _mock = new Mock<IUserRepository>();

            _mock.Setup(r => r.GetCurrentUser("admin@admin.ru", It.IsAny<bool>())).Returns(user);
        }

        public Mock<IUserRepository> Mock() => _mock;

        public IUserRepository MockObject() => _mock.Object;
    }
}