using System;
using System.Collections.Generic;
using Server.Data.Interfaces;
using Server.Exceptions;
using Server.Infrastructure;
using Server.Models;
using Server.Services.Converters;
using Server.Services.Interfaces;

// ReSharper disable PossibleMultipleEnumeration

namespace Server.Services
{
    /// <inheritdoc />
    public class BankService : IBankService
    {
        private readonly ICardRepository _cardRepository;
        private readonly ICardService _cardService;
        private readonly IBusinessLogicValidationService _businessLogicValidationService;
        private readonly ICurrencyConverter _currencyConverter;
        private readonly ICardNumberGenerator _cardNumberGenerator;

        /// <inheritdoc />
        public BankService(
            ICardRepository cardRepository,
            ICardService cardService,
            IBusinessLogicValidationService businessLogicValidationService,
            ICurrencyConverter currencyConverter,
            ICardNumberGenerator cardNumberGenerator)
        {
            _cardRepository = cardRepository ?? throw new ArgumentNullException(nameof(cardRepository));
            _cardService = cardService ?? throw new ArgumentNullException(nameof(cardService));
            _businessLogicValidationService = businessLogicValidationService ??
                                              throw new ArgumentNullException(nameof(businessLogicValidationService));
            _currencyConverter = currencyConverter ?? throw new ArgumentNullException(nameof(currencyConverter));
            _cardNumberGenerator = cardNumberGenerator ?? throw new ArgumentNullException(nameof(cardNumberGenerator));
        }

        /// <inheritdoc />
        public (Card, IEnumerable<CustomModelError>) TryOpenNewCard(
            User user,
            string shortCardName,
            Currency currency,
            CardType cardType)
        {
            // Validation
            var validationResult = new List<CustomModelError>();
            validationResult.AddError(() => cardType == CardType.OTHER,
                "type", "Wrong type card", "Неверный тип карты", TypeCriticalException.CARD);

            if (validationResult.HasErrors()) return (null, validationResult);

            // Select
            var allCards = _cardRepository.GetCards(user);

            var cardNumber = _cardNumberGenerator.GenerateNewCardNumber(cardType);

            // Validation
            var cardExisting = _businessLogicValidationService.ValidateCardExist(allCards, shortCardName, cardNumber);
            validationResult.AddError(() => cardExisting,
                "internal", "Card exist", "Карта с таким номером уже существует", TypeCriticalException.CARD);

            if (validationResult.HasErrors()) return (null, validationResult);

            // Create
            var newCard = new Card
            {
                CardNumber = cardNumber,
                CardName = shortCardName,
                Currency = currency,
                CardType = cardType
            };

            try
            {
                allCards.Add(newCard);
                var addBonusOnOpenFlag = _cardService.TryAddBonusOnOpen(newCard);

                validationResult.AddError(() => !addBonusOnOpenFlag,
                    "internal", "Add bonus to card failed", "Ошибка при открытии карты", TypeCriticalException.CARD);

                if (validationResult.HasErrors())
                {
                    allCards.Remove(newCard);
                    return (null, validationResult);
                }
            }
            catch (Exception e)
            {
                allCards.Remove(newCard);
                return (null,
                    validationResult.AddError("internal", e.Message ?? "Internal error", "Что то пошло не так",
                        TypeCriticalException.CARD));
            }

            return (newCard, validationResult);
        }

        /// <inheritdoc />
        public (Transaction, IEnumerable<CustomModelError>) TryTransferMoney(User user, decimal sum, string from,
            string to)
        {
            // Select
            var cardFrom = _cardRepository.GetCard(user, from);
            var cardTo = _cardRepository.GetCard(user, to);

            // Validating
            var validationResult = _businessLogicValidationService.ValidateTransfer(cardFrom, cardTo, sum);

            if (validationResult.HasErrors()) return (null, validationResult);

            // Transfer
            var fromTransaction = new Transaction
            {
                Card = cardFrom,
                CardFromNumber = cardFrom.CardNumber,
                CardToNumber = cardTo.CardNumber,
                Sum = sum
            };

            var toTransaction = new Transaction
            {
                Card = cardTo,
                DateTime = fromTransaction.DateTime,
                CardFromNumber = cardFrom.CardNumber,
                CardToNumber = cardTo.CardNumber,
                Sum = _currencyConverter.GetConvertedSum(sum, cardFrom.Currency, cardTo.Currency)
            };

            try
            {
                cardFrom.Transactions.Add(fromTransaction);
                cardTo.Transactions.Add(toTransaction);
            }
            catch (Exception e)
            {
                return (null,
                    new List<CustomModelError>().AddError("internal", e.Message ?? "Internal error",
                        "Что то пошло не так"));
            }

            return (fromTransaction, validationResult);
        }
    }
}