using System;
using Server.Services.Checkers;
using Server.Services;
using Server.Infrastructure;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            string CardNumber;
            Console.Write("Card number: ");
            CardNumber = Console.ReadLine();
            CardChecker checker = new CardChecker();
            CardService service = new CardService();
            CardType cardtype = service.GetCardType(CardNumber);
            Console.Write("Card number is ");
            if (!checker.CheckCardNumber(CardNumber))
            {
                Console.WriteLine("invalid");
            }
            else
            {
                Console.WriteLine("valid");
                Console.WriteLine(String.Format("Card type is {0}", Enum.GetName(typeof(CardType), cardtype)));
                if (checker.CheckCardEmitter(CardNumber))
                {
                    Console.WriteLine("Card Emitter - Alfa Bank");
                }
                else
                {
                    Console.WriteLine("Unknow card emitter");
                }
            }
        }
    }
}