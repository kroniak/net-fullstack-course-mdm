using Server.Infrastructure;

namespace Server.Services.Interfaces
{
    /// <summary>
    /// Service to generating new card number 
    /// </summary>
    public interface ICardNumberGenerator
    {
        /// <summary>
        /// Generate new card number from BIN list
        /// </summary>
        /// <param name="cardType">Type of the card</param>
        /// <returns>Generated new card number</returns>
        string GenerateNewCardNumber(CardType cardType);
    }
}