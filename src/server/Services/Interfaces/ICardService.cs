using Server.Infrastructure;
using Server.Models;

namespace Server.Services.Interfaces
{
    /// <summary>
    /// Interface for checking numbers and extracting card types
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
        /// Add bonus to new card when its opening
        /// </summary>
        /// <param name="card">Card to</param>
        /// <returns>Return <see langword="bool"/> if operation is successfully</returns>
        bool TryAddBonusOnOpen(Card card);
    }
}