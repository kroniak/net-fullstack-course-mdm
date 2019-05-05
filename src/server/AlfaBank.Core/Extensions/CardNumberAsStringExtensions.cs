using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace AlfaBank.Core.Extensions
{
    public static class CardNumberAsStringExtensions
    {
        /// <summary>
        /// Normalize card number (from any format to only digits)
        /// </summary>
        /// <param name="cardNumber">card number in any format</param>
        /// <returns>Digits of a card number</returns>
        public static string ToNormalizedCardNumber(this string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber)) return null;

            var resultNumbers = new StringBuilder();

            foreach (var item in cardNumber)
                if (char.IsDigit(item))
                    resultNumbers.Append(item);

            return resultNumbers.ToString();
        }

        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public static string CardNumberWatermark(this string number)
        {
            number = number.ToNormalizedCardNumber();

            if (number == null) return null;

            var result = number.Substring(0, 4);
            result = result + "XXXXXXXX";
            return result + number.Substring(number.Length - 4, 4);
        }
    }
}