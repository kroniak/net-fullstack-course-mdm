using AlfaBank.Core.Exceptions;
using AlfaBank.Core.Models;
using System;
using Xunit;

namespace Server.Test.Models
{
    public class UserTest
    {
        [Theory]
        [InlineData(null, null)]
        [InlineData(" ", " ")]
        [InlineData("", "")]
        public void User_NullEmail_ThrowException(string username, string password) =>
            Assert.Throws<CriticalException>(() => new User(username, password));

        [Theory]
        [InlineData("admin")]
        [InlineData("admin@")]
        public void SetUserEmail_IncorrectEmail_ThrowException(string s) =>
            Assert.Throws<CriticalException>(() => new User(s, "123"));

        [Fact]
        public void User_CorrectData_ReturnCorrectUser()
        {
            // Act
            var user = new User("alice@alfabank.ru", "123");

            Assert.Equal("alice@alfabank.ru", user.UserName);
            Assert.Equal("123", user.Password);
            Assert.Null(user.Firstname);
            Assert.Null(user.Surname);
            Assert.Null(user.Birthday);
            Assert.Null(user.Cards);
        }

        [Fact]
        public void User_CorrectFullData_ReturnCorrectUser()
        {
            // Arrange
            const string firstname = "John";
            const string surname = "Smith";
            var birthday = DateTime.Today;

            // Act
            var user = new User("alice@alfabank.ru", "123")
            {
                Firstname = firstname,
                Surname = surname,
                Birthday = birthday
            };

            Assert.Equal("alice@alfabank.ru", user.UserName);
            Assert.Equal(firstname, user.Firstname);
            Assert.Equal(surname, user.Surname);
            Assert.Equal(birthday, user.Birthday);
            Assert.Null(user.Cards);
        }
    }
}