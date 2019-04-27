using System;
using AlfaBank.Core.Exceptions;
using AlfaBank.Core.Models;
using Xunit;

namespace Server.Test.Models
{
    public class UserTest
    {
        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("")]
        public void User_NullEmail_ThrowException(string s) => Assert.Throws<CriticalException>(() => new User(s));

        [Theory]
        [InlineData("admin")]
        [InlineData("admin@")]
        public void SetUserEmail_IncorrectEmail_ThrowException(string s) =>
            Assert.Throws<CriticalException>(() => new User(s));

        [Fact]
        public void User_CorrectData_ReturnCorrectUser()
        {
            // Act
            var user = new User("admin@admin.ru");

            Assert.Equal("admin@admin.ru", user.UserName);
            Assert.Null(user.Firstname);
            Assert.Null(user.Surname);
            Assert.Null(user.Birthday);
            Assert.Empty(user.Cards);
        }

        [Fact]
        public void User_CorrectFullData_ReturnCorrectUser()
        {
            // Arrange
            const string firstname = "John";
            const string surname = "Smith";
            var birthday = DateTime.Today;

            // Act
            var user = new User("admin@admin.ru")
            {
                Firstname = firstname,
                Surname = surname,
                Birthday = birthday
            };

            Assert.Equal("admin@admin.ru", user.UserName);
            Assert.Equal(firstname, user.Firstname);
            Assert.Equal(surname, user.Surname);
            Assert.Equal(birthday, user.Birthday);
            Assert.Empty(user.Cards);
        }
    }
}