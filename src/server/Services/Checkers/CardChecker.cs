namespace Server.Services.Checkers
{
    /// <inheritdoc />
    /// <summary>
    /// Interface for checking numbers card types
    /// </summary>
    public class CardChecker : ICardChecker
    {
        /// <inheritdoc />
        /// <summary>
        /// Check card number by Alfabank emitter property
        /// </summary>
        /// <param name="number">card number in any format</param>
        /// <returns>Return <see langword="true" /> if card was emitted in Alfabank </returns>
        public bool CheckCardEmitter(string number) => throw new System.NotImplementedException();

        /// <inheritdoc />
        /// <summary>
        /// Extract card number
        /// </summary>
        /// <param name="number">card number in any format</param>
        /// <returns>Return 0 is card is invalid, 1 if card is mastercard, 2 is visa, 3 is maestro, 4 is visa electon</returns>
        public bool CheckCardNumber(string number) => throw new System.NotImplementedException();
    }
}