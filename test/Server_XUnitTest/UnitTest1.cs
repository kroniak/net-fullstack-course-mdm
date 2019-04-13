using System;
using Xunit;
using Server.Services.Checkers;

namespace Server_XUnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test_CheckCardNumber_TRUE()
        {
            // правильные номера карт
            CardChecker luhn_checks = new CardChecker();

            Assert.True(luhn_checks.CheckCardNumber("371023430676356"));
            Assert.True(luhn_checks.CheckCardNumber("4539804056541904"));
            Assert.True(luhn_checks.CheckCardNumber("5190967013969042"));
            Assert.True(luhn_checks.CheckCardNumber("2200330205300040078")); //spb мир
        }
        [Fact]
        public void Test_CheckCardNumber_FALSE()
        {
            //неправельные номера  карт
            CardChecker luhn_checks = new CardChecker();
            Assert.False(luhn_checks.CheckCardNumber("4890420010407712")); //unicredit
            Assert.False(luhn_checks.CheckCardNumber("4276550011535642")); //sber
            Assert.False(luhn_checks.CheckCardNumber("4790878824195158")); //alfa
            Assert.False(luhn_checks.CheckCardNumber("00п"));


        }

        [Fact]
        public void Test_CheckCardEmitter_TRUE()
        {
            //
            CardChecker luhn_checks = new CardChecker();
            Assert.True(luhn_checks.CheckCardEmitter("4790878824195565"));
        }
        [Fact]
        public void Test_CheckCardEmitter_FALSE()
        {
            //
            CardChecker luhn_checks = new CardChecker();
            Assert.False(luhn_checks.CheckCardEmitter("4890878824195565"));
        }
        [Fact]
        public void Test_GetCardMII_TRUE()
        {
            //
            CardChecker luhn_checks = new CardChecker();
            var z = luhn_checks.GetCardMII("4790878824195565");
            Assert.True(z.Equals(2));
            z = luhn_checks.GetCardMII("5790878824195565");
            Assert.True(z.Equals(1));
            z = luhn_checks.GetCardMII("5018878824195565");
            Assert.True(z.Equals(3));
            z = luhn_checks.GetCardMII("5020878824195565");
            Assert.True(z.Equals(3));
            z = luhn_checks.GetCardMII("4844878824195565");
            Assert.True(z.Equals(4));

        }
    }
}