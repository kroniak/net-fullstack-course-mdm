using Server.Models;

namespace Server.Services.Checkers
{
    /// <summary>
    /// Checking methods fro cards and their numbers
    /// </summary>
    public interface ICardChecker
    {
        /// <summary>
        /// Check card number by Lun algorithm
        /// </summary>
        /// <param name="number">card number in any format</param>
        /// <returns>Return <see langword="true"/> if card is valid</returns>
        bool CheckCardNumber(string number);

        /// <summary>
        /// Check card number by Alfabank emitter property
        /// </summary>
        /// <param name="number">card number in any format</param>
        /// <returns>Return <see langword="true"/> if card was emitted in Alfabank</returns>
        bool CheckCardEmitter(string number);

        /// <summary>
        /// Check Card expired or not
        /// </summary>
        /// <param name="card">Card for checking</param>
        /// <returns>Return <see langword="true"/> if card is active</returns>
        bool CheckCardActivity(Card card);
    }
}