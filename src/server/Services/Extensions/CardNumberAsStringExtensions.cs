using System.Text;

namespace Server.Services.Extensions
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
            var resultNumbers = new StringBuilder();

            foreach (var item in cardNumber)
                if (char.IsDigit(item))
                    resultNumbers.Append(item);

            return resultNumbers.ToString();
        }

        public static string CardNumberWatermark(this string number)
        {
            if (string.IsNullOrWhiteSpace(number)) return null;

            var result = number.Substring(0, 4);
            result = result + "XXXXXXXX";
            return result + number.Substring(number.Length - 4, 4);
        }
    }
}