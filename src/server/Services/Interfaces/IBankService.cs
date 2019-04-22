using System.Collections.Generic;
using Server.Exceptions;
using Server.Infrastructure;
using Server.Models;

namespace Server.Services.Interfaces
{
    /// <summary>
    /// Service for transferring money, opening card and other operation
    /// </summary>
    public interface IBankService
    {
        /// <summary>
        /// Open new card for current user
        /// </summary>
        /// <param name="user"> current user</param>
        /// <param name="shortCardName"></param>
        /// <param name="currency"></param>
        /// <param name="cardType"></param>
        /// <returns>new <see cref="Card"/> object</returns>
        (Card, IEnumerable<CustomModelError>)
            TryOpenNewCard(User user, string shortCardName, Currency currency, CardType cardType);

        /// <summary>
        /// Transfer money
        /// </summary>
        /// <param name="user"> current user</param>
        /// <param name="sum">sum of operation</param>
        /// <param name="from">card number</param>
        /// <param name="to">card number</param>
        /// <returns>new <see cref="Transaction"/> object</returns>
        (Transaction, IEnumerable<CustomModelError>) TryTransferMoney(User user, decimal sum, string from, string to);
    }
}