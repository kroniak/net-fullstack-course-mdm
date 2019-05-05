using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AlfaBank.Core.Exceptions;
using AlfaBank.Core.Extensions;
using AlfaBank.Core.Models;
using AlfaBank.Services.Checkers;
using AlfaBank.Services.Interfaces;

// ReSharper disable ImplicitlyCapturedClosure

namespace AlfaBank.Services
{
    /// <inheritdoc />
    public class BusinessLogicValidationService : IBusinessLogicValidationService
    {
        private readonly ICardChecker _cardChecker;

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        public BusinessLogicValidationService(ICardChecker cardChecker)
        {
            _cardChecker = cardChecker ?? throw new ArgumentNullException(nameof(cardChecker));
        }

        /// <inheritdoc />
        public IEnumerable<CustomModelError> ValidateTransfer(Card from, Card to, decimal sum)
        {
            var result = new List<CustomModelError>();

            result
                .AddError(() => from == null,
                    "from",
                    "Card not found",
                    "Карта не найдена",
                    TypeCriticalException.CARD)
                .AddError(() => to == null,
                    "to",
                    "Card not found",
                    "Карта не найдена",
                    TypeCriticalException.CARD)
                .AddError(() => from.CardNumber == to.CardNumber,
                    "from",
                    "From card and to card is Equal",
                    "Нельзя перевести на ту же карту",
                    TypeCriticalException.CARD)
                .AddError(() => !_cardChecker.CheckCardActivity(from),
                    "from", "Card is expired", "Карта просрочена",
                    TypeCriticalException.CARD)
                .AddError(() => !_cardChecker.CheckCardActivity(to),
                    "to",
                    "Card is expired", "Карта просрочена",
                    TypeCriticalException.CARD)
                .AddError(() => from.Balance < sum,
                    "from",
                    "Balance of the card is low",
                    "Нет денег на карте",
                    TypeCriticalException.CARD);

            return result;
        }
    }
}