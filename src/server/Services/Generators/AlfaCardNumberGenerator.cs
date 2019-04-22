using System;
using System.Linq;
using Server.Exceptions;
using Server.Infrastructure;
using Server.Services.Interfaces;

namespace Server.Services.Generators
{
    /// <inheritdoc />
    public class AlfaCardNumberGenerator : ICardNumberGenerator
    {
        /// <inheritdoc />
        public string GenerateNewCardNumber(CardType cardType)
        {
            var startBin = "51";

            switch (cardType)
            {
                case CardType.MIR:
                    startBin = "2";
                    break;
                case CardType.VISA:
                    startBin = "4";
                    break;
                case CardType.MAESTRO:
                    startBin = "6";
                    break;
                case CardType.MASTERCARD:
                    startBin = "51";
                    break;
                case CardType.OTHER:
                    return null;
                default:
                    startBin = "51";
                    break;
            }

            startBin = Constants.AlfaBins.First(x => x.StartsWith(startBin));
            if (string.IsNullOrWhiteSpace(startBin))
                throw new CriticalException("Cannot create new card number", TypeCriticalException.CARD);

            // generate new CardNumber for user
            return GenerateNewCardNumber(startBin);
        }

        private static string GenerateNewCardNumber(string prefix, int length = 16)
        {
            var ccNumber = prefix;

            while (ccNumber.Length < length - 1)
            {
                var rnd = new Random().NextDouble() * 1.0f - 0f;
                ccNumber += Math.Floor(rnd * 10);
            }

            // reverse number and convert to int
            var reversedCcNumberString = ccNumber.ToCharArray().Reverse();
            var reversedCcNumberList = reversedCcNumberString.Select(c => Convert.ToInt32(c.ToString()));

            // calculate sum
            var sum = 0;
            var pos = 0;
            var reversedCcNumber = reversedCcNumberList.ToArray();

            while (pos < length - 1)
            {
                var odd = reversedCcNumber[pos] * 2;

                if (odd > 9)
                    odd -= 9;

                sum += odd;

                if (pos != length - 2)
                    sum += reversedCcNumber[pos + 1];

                pos += 2;
            }

            // calculate check digit
            var checkDigit =
                Convert.ToInt32((Math.Floor((decimal) sum / 10) + 1) * 10 - sum) % 10;

            ccNumber += checkDigit;

            return ccNumber;
        }
    }
}