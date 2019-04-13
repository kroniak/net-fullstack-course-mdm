using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Server.Services.Checkers
{
    /// <inheritdoc />
    /// <summary>
    /// Interface for checking numbers card types
    /// </summary>


public class CardChecker : ICardChecker
    {
        
        /// <inheritdoc />
        /// <summary>
        /// Check card number by Alfabank emitter property
        /// </summary>
        /// <param name="number">card number in any format</param>
        /// <returns>Return <see langword="true" /> if card was emitted in Alfabank </returns>
        public bool CheckCardEmitter(string number)
        {
            //Проверим что альфабанк
            return Regex.IsMatch(number, @"^[0-9]{1}79087[0-9]");
        }

   
        public bool CheckCardNumber(string number)
        {
            return number.All(char.IsDigit) && number.Reverse()
            .Select(c => c - 48)
            .Select((thisNum, i) => i % 2 == 0
                ? thisNum
                : ((thisNum *= 2) > 9 ? thisNum - 9 : thisNum)
            ).Sum() % 10 == 0;

        }
        /// <inheritdoc />
        /// <summary>
        /// Extract card number
        /// </summary>
        /// <param name="number">card number in any format</param>
        /// <returns>Return 0 is card is invalid, 1 if card is mastercard, 2 is visa, 3 is maestro, 4 is visa electon</returns>
        public int GetCardMII(string number)
        {
            if (Regex.IsMatch(number, @"^5018|5020|5038|5893|6304|6759[0-9]$") == true)
                return 3; //maestro
            else if (Regex.IsMatch(number, @"^51|52|53|54|55[0-9]$") == true)
                return 1; //mastercard
            else if (Regex.IsMatch(number, @"^4026|417500|4508|4844|4913|4917[0-9]$") == true)
                return 4; //visa electon
            else if(Regex.IsMatch(number, @"^[4]{1}[0-9]") == true)
                return 2; //visa
            else if (Regex.IsMatch(number, @"^[5]{1}[0-9]") == true)
                return 1; //mastercard
            else return 0;
        }
    }
}