namespace Server.Services.Checkers
{
    /// <summary>
    /// Interface for checking numbers card types
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
        /// <returns>Return <see langword="true"/> if card was emitted in Alfabank </returns>
        bool CheckCardEmitter(string number);
        int GetCardMII(string number);
    }
}