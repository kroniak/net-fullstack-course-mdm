using Server.Infrastructure;
using Server.Models;

namespace Server.Services
{
    /// <summary>
    /// Interface for generating numbers and extracting card types
    /// </summary>
    public interface ICardService
    {
        /// <summary>
        /// Extract card number
        /// </summary>
        /// <param name="number">card number in any format</param>
        /// <returns>Return enum CardType</returns>
        CardType GetCardType(string number);

        /// <summary>
        /// Generate new card number from BIN list
        /// </summary>
        /// <param name="cardType">Type of the card</param>
        /// <returns>Generated new card number</returns>
        string GenerateNewCardNumber(CardType cardType);

        /// <summary>
        /// Add bonus to new card when its opening
        /// </summary>
        /// <param name="card">Card to</param>
        /// <returns>Return <see langword="bool"/> if operation is successfully</returns>
        bool TryAddBonusOnOpen(Card card);

        /// <summary>
        /// Get balance of the card
        /// </summary>
        /// <param name="card">Card to calculating</param>
        /// <returns><see langword="decimal"/> sum</returns>
        decimal GetBalanceOfCard(Card card);
    }
}