using System;
using System.Collections.Generic;

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
            List<string> BINs = new List<string>() { "521178", "548673", "548601", "552175", "415428", "477964"};
            string BIN = number.Substring(0, 6);
            return BINs.Contains(BIN);
        }

        /// <inheritdoc />
        /// <summary>
        /// Extract card number
        /// </summary>
        /// <param name="number">card number in any format</param>
        /// <returns>Return 0 is card is invalid, 1 if card is mastercard, 2 is visa, 3 is maestro, 4 is visa electon</returns>
        public bool CheckCardNumber(string number)
        {

            int numLength = number.Length - 1;
            int num = 0;
            int sum = 0;

            for (int i = 0; i <= numLength; i++)
            {
                num = int.Parse(number[i].ToString());
                if (numLength % 2 == 0)
                {
                    num *= 2;
                    if (num > 9)
                    {
                        num -= 9;
                    }
                }
                sum += num;
            }
            return sum % 10 == 0;
        }
    }
}
