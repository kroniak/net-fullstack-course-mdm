using Moq;
using Server.Data.Interfaces;
using Server.Models;

namespace ServerTest.Mocks.Data
{
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