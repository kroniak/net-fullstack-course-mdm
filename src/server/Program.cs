using System;
using Server.Services.Checkers;

namespace Server
{
    class Program
    {
        private static readonly ICardChecker CardChecker = new CardChecker();

        static void Main(string[] args)
        {
            const string cardNumber = "1234 1234 1233 1234";
            var result = CardChecker.CheckCardNumber(cardNumber);
            Console.WriteLine($"Test for card {cardNumber} is {result}");
        }
    }
}