using System.Collections.Generic;
using AlfaBank.Core.Exceptions;
using AlfaBank.Core.Models;

namespace AlfaBank.Services.Interfaces
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
    }
}