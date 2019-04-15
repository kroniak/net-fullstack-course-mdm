using System;
using System.Collections.Generic;
using System.Linq;

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
			return number.Substring(1, 3) == "234";
        }

        /// <inheritdoc />
        /// <summary>
        /// Check card number by Lun algorithm
        /// </summary>
        /// <param name="number">card number in any format</param>
        /// <returns>Return <see langword="true"/> if card is valid</returns>
        public bool CheckCardNumber(string number)
        {
            var numberList = number
				.ToArray()
				.Select(x => Int32.Parse(x.ToString()))
				.ToList();
            var startNumber = 0;
            if(number.Length % 2 != 0)
            {
                startNumber = 1;
            }

            for(int i = startNumber; i < number.Length; i+=2)
            {
                numberList[i] = numberList[i] * 2;
                if(numberList[i] > 9)
                {
                   numberList[i] = numberList[i] - 9; 
                }
            }

            return numberList.Sum() % 10 == 0;
        }
    }
}