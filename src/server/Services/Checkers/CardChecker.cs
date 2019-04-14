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
			var bin = number.Substring(0, 6);
			var alfabankBinList = new List<string>()
			{
				"415428",
				"477964",
				"521178",
				"548601",
				"548673",
				"676371"
			};
			return alfabankBinList.Contains(bin);
        }

        /// <inheritdoc />
        /// <summary>
        /// Check card number by Lun algorithm
        /// </summary>
        /// <param name="number">card number in any format</param>
        /// <returns>Return <see langword="true"/> if card is valid</returns>
        public bool CheckCardNumber(string number)
        {
            var numberList = number.ToCharArray().Select(x => (int)x).ToList();
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