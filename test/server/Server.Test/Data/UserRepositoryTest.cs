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

        [Fact]
        public void GetCurrentUser_ReturnCorrectUser()
        {
            // Act
            var user = _userRepository.GetCurrentUser("admin@admin.ru");

            // Assert
            Assert.Equal("admin@admin.ru", user.UserName);
        }
    }
}