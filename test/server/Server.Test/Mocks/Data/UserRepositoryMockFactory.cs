using System.Diagnostics.CodeAnalysis;
using AlfaBank.Core.Data.Interfaces;
using AlfaBank.Core.Models;
using Moq;

namespace Server.Test.Mocks.Data
{
    [ExcludeFromCodeCoverage]
    public class UserRepositoryMockFactory : IMockFactory<IUserRepository>
    {
        private readonly Mock<IUserRepository> _mock;

        public UserRepositoryMockFactory(User user)
        {
            _mock = new Mock<IUserRepository>();

            _mock.Setup(r => r.GetCurrentUser()).Returns(user);
        }

        public Mock<IUserRepository> Mock() => _mock;

        public IUserRepository MockObject() => _mock.Object;
    }
}