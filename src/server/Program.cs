
using Server.Services.Checkers;
using System;

namespace Server
{
  
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Alfalab C# test app");
            CardChecker luhn_checks = new CardChecker();
            Console.WriteLine("---" + luhn_checks.GetCardMII("5018878824195565"));

            
            Console.Write("Press <Enter>");
            Console.ReadLine();
        }
    }
}