using System;
using System.Diagnostics.CodeAnalysis;
using AlfaBank.Core.Data.Interfaces;
using AlfaBank.Core.Infrastructure;
using AlfaBank.Core.Models;
using AlfaBank.Services.Checkers;
using AlfaBank.Services.Converters;
using AlfaBank.Services.Interfaces;

namespace AlfaBank.Services
{
    /// <inheritdoc />
    public class CardService : ICardService
    {
        private readonly ICardChecker _cardChecker;
        private readonly ICurrencyConverter _currencyConverter;
        private readonly ICardRepository _cardRepository;

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        public CardService(
            ICardChecker cardChecker,
            ICurrencyConverter currencyConverter,
            ICardRepository cardRepository)
        {
            _cardChecker = cardChecker ??
                           throw new ArgumentNullException(nameof(cardChecker));
            _currencyConverter = currencyConverter ??
                                 throw new ArgumentNullException(nameof(currencyConverter));
            _cardRepository = cardRepository ?? throw new ArgumentNullException(nameof(cardRepository));
        }

        /// <inheritdoc />
        public CardType GetCardType(string number)
        {
            if (!_cardChecker.CheckCardNumber(number)) return CardType.OTHER;

            var firstDigit = number[0];
            var secondDigit = number[1];

            switch (firstDigit)
            {
                case '2':
                    return CardType.MIR;
                case '4':
                    return CardType.VISA;
                case '5'
                    when secondDigit == '0' || secondDigit > '5':
                    return CardType.MAESTRO;
                case '5'
                    when secondDigit >= '1' && secondDigit <= '5':
                    return CardType.MASTERCARD;
                case '6':
                    return CardType.MAESTRO;
                default:
                    return CardType.OTHER;
            }
        }

        /// <inheritdoc />
        public bool TryAddBonusOnOpen(Card card)
        {
            if (card.Transactions == null) return false;

            try
            {
                card.Transactions.Add(new Transaction
                {
                    Card = card,
                    CardToNumber = card.CardNumber,
                    Sum = _currencyConverter.GetConvertedSum(10M, Currency.RUR, card.Currency)
                });
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc />
        /// <summary>
        /// Try to charge tariff from card
        /// </summary>
        /// <param name="card">Card from</param>
        /// <param name="tariff">Tariff in RUB</param>
        /// <returns>Return <see langword="bool" /> if operation is successfully</returns>
        public bool TryTariffCharge(Card card, decimal tariff)
        {
            if (tariff <= 0M)
            {
                return false;
            }

            try
            {
                var tariffInCurrency = _currencyConverter.GetConvertedSum(tariff, Currency.RUR, card.Currency);
                if (card.Balance >= tariffInCurrency)
                {
                    card.Transactions.Add(new Transaction
                    {
                        Card = card,
                        CardFromNumber = card.CardNumber,
                        Sum = tariffInCurrency
                    });
                    _cardRepository.Update(card);
                    _cardRepository.Save();
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}