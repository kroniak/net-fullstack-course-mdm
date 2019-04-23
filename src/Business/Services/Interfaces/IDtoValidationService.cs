using System.Collections.Generic;
using Models.Exceptions;
using Models.Dto;

namespace Business.Services.Interfaces
{
    /// <summary>
    /// Service for validation DTO
    /// </summary>
    public interface IDtoValidationService
    {
        /// <summary>
        /// Method to validate transfer
        /// </summary>
        /// <param name="transaction">transaction DTO</param>
        IEnumerable<CustomModelError> ValidateTransferDto(TransactionPostDto transaction);

        /// <summary>
        /// Method to validate card dto
        /// </summary>
        /// <param name="card">card DTO</param>
        IEnumerable<CustomModelError> ValidateOpenCardDto(CardPostDto card);
    }
}