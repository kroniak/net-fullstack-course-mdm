using Server.Infrastructure;

namespace Server.Services
{
    /// <inheritdoc />
    /// <summary>
    /// Our implementing of the <see cref="T:Server.Services.ICardService" /> interface
    /// </summary>
    public class CardService : ICardService
    {
        /// <inheritdoc />
        /// <summary>
        /// Get card type by card number
        /// </summary>
        /// <param name="number">card number in any format</param>
        /// <returns>Return enum CardType</returns>
        public CardType GetCardType(string number)
        {
            switch(number[0])
            {
                case '2':
                    return CardType.MIR;
                case '4':
                    return CardType.VISA;
                case '3':
                case '5':
                case '6':
                    switch(number[1])
                    {
                        case '0':
                        case '6':
                        case '7':
                        case '8':
                            return CardType.MAESTRO;
                        default:
                            return CardType.MASTERCARD;
                    }
                default:
                    return CardType.OTHER;
            }
        }
    }
}