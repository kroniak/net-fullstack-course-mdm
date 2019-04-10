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
        public CardType GetCardType(string number) => throw new System.NotImplementedException();
    }
}