using Server.Infrastructure;

namespace Server.Services
{
    /// <summary>
    /// Service for transferring money^ opening card and other operation
    /// </summary>
    public interface IBankService
    {
        /// <summary>
        /// Transfer money
        /// </summary>
        /// <param name="sum">sum of operation</param>
        /// <param name="from">card number</param>
        /// <param name="to">card number</param>
        /// <returns>Returns <see langword="True"/> if transfer is successful</returns>
        bool TryTransferMoney(decimal sum, string from, string to);

        /// <summary>
        /// OpenNewCard
        /// </summary>
        /// <param name="cardType">type of the cards</param>
        /// <returns>Returns <see langword="True"/> if result of card opening is successfully</returns>
        bool TryOpenNewCard(CardType cardType);
    }
}