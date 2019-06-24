using AlfaBank.Core.Data.Interfaces;
using AlfaBank.Core.Data.Repositories;
using Server.Test.Mocks;
using Xunit;

// ReSharper disable PossibleMultipleEnumeration
namespace Server.Test.Data
{
    public class UserRepositoryTest
    {
        private readonly IUserRepository _userRepository;

        public UserRepositoryTest()
        {
            _userRepository = new UserRepository(SqlContextMock.GetSqlContext());
        }

        [Theory]
        [InlineData("alice@alfabank.ru")]
        [InlineData("bob@alfabank.ru")]
        public void GetCurrentUser_ReturnCorrectUser(string userName)
        {
            // Act
            var user = _userRepository.GetUser(userName);

            // Assert
            Assert.Equal(userName, user.UserName);
        }
    }
}