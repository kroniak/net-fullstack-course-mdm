using System;
using System.Collections.Generic;
using Server.Exceptions;
using Server.Infrastructure;
using Server.Models.Dto;
using Server.Services.Checkers;
using Server.Services.Extensions;
using Server.Services.Interfaces;

// ReSharper disable ImplicitlyCapturedClosure

namespace Server.Services
{
    /// <inheritdoc />
    public class DtoValidationService : IDtoValidationService
    {
        private readonly ICardChecker _cardChecker;

        public DtoValidationService(ICardChecker cardChecker)
        {
            _cardChecker = cardChecker ??
                           throw new ArgumentNullException(nameof(cardChecker));
        }

        /// <inheritdoc />
        public IEnumerable<CustomModelError> ValidateTransferDto(TransactionPostDto transaction)
        {
            var result = new List<CustomModelError>();

            result
                .AddError(() =>
                        transaction.From.ToNormalizedCardNumber() ==
                        transaction.To.ToNormalizedCardNumber(),
                    "from",
                    "From card and to card is Equal",
                    "Нельзя перевести на ту же карту",
                    TypeCriticalException.CARD)
                .AddError(() => !_cardChecker.CheckCardEmitter(transaction.From),
                    "from",
                    "Card number is invalid",
                    "Номер карты неверный",
                    TypeCriticalException.CARD)
                .AddError(() => !_cardChecker.CheckCardEmitter(transaction.To),
                    "to",
                    "Card number is invalid",
                    "Номер карты неверный",
                    TypeCriticalException.CARD)
                .AddError(() => transaction.Sum <= 0,
                    "sum",
                    "Sum must be greater then 0",
                    "Сумма должна быть больше 0");

            return result;
        }

        /// <inheritdoc />
        public IEnumerable<CustomModelError> ValidateOpenCardDto(CardPostDto card)
        {
            var result = new List<CustomModelError>();

            result
                .AddError(() => card.Type <= 0 || card.Type > 4,
                    "type",
                    "Card type is invalid",
                    "Тип карты неверный",
                    TypeCriticalException.CARD)
                .AddError(() => card.Currency < 0 || card.Currency > 2,
                    "currency",
                    "Currency is invalid",
                    "Валюта карты неверная",
                    TypeCriticalException.CARD);

            return result;
        }
    }
}