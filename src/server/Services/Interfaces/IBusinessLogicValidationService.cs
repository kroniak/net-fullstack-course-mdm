using System.Collections.Generic;
using Server.Exceptions;
using Server.Models;

namespace Server.Services.Interfaces
{
    /// <summary>
    /// Business Logic Validation Service
    /// </summary>
    public interface IBusinessLogicValidationService
    {
        /// <summary>
        /// Validate cards and their balance
        /// </summary>
        /// <param name="from">card from</param>
        /// <param name="to">card to</param>
        /// <param name="sum">sum</param>
        IEnumerable<CustomModelError> ValidateTransfer(Card from, Card to, decimal sum);

        /// <summary>
        /// Validate exist card by number or by name
        /// </summary>
        /// <param name="cards">cards</param>
        /// <param name="shortCardName">name</param>
        /// <param name="cardNumber">number</param>
        bool ValidateCardExist(IEnumerable<Card> cards, string shortCardName, string cardNumber);
    }
}