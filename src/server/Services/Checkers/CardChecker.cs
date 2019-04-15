using System;
using System.Collections.Generic;
using System.Linq;
using Server.Infrastructure;
using Server.Models;
using Server.Services.Extensions;

namespace Server.Services.Checkers
{
    public class CardChecker : ICardChecker
    {
        /// <inheritdoc />
        /// <summary>
        /// Check card number by Lun algorithm
        /// </summary>
        /// <param name="number">card number in any format</param>
        /// <returns>Return <see langword="true" /> if card is valid</returns>
        public bool CheckCardNumber(string number)
        {
            if (string.IsNullOrWhiteSpace(number)) return false;

            var cardNumber = number.ToNormalizedCardNumber();

            if (cardNumber.Length < 12 && cardNumber.Length > 19) return false;

            var intNumbers = CreateIntCollectionFromString(cardNumber);

            var checkList = new List<int>();

            for (var i = 0; i < intNumbers.Count; i += 2)
            {
                var digit = intNumbers[i] * 2;
                digit = digit > 9 ? digit - 9 : digit;
                checkList.Add(digit);
            }

            for (var i = 1; i < intNumbers.Count; i += 2)
                checkList.Add(intNumbers[i]);

            var controlSum = 0;
            foreach (var item in checkList)
                controlSum += item;

            return controlSum % 10 == 0;
        }

        /// <inheritdoc />
        /// <summary>
        /// Check card number by Alfabank emitter property
        /// </summary>
        /// <param name="number">card number in any format</param>
        /// <returns>Return <see langword="true" /> if card was emitted in Alfabank </returns>
        public bool CheckCardEmitter(string number) =>
            CheckCardNumber(number) && Constants.AlfaBins.Any(number.StartsWith);

        /// <inheritdoc />
        /// <summary>
        /// Check Card expired or not
        /// </summary>
        /// <param name="card">Card for checking</param>
        /// <returns>Return <see langword="true" /> if card is active</returns>
        public bool CheckCardActivity(Card card) =>
            throw new NotImplementedException();

        #region Utils

        /// <summary>
        /// Utils method. Create collection of ints from valid card string
        /// </summary>
        /// <param name="numbers">Valid card number</param>
        /// <returns>Collection of Int</returns>
        private static IList<int> CreateIntCollectionFromString(string numbers)
        {
            var result = new List<int>(numbers.Length);

            foreach (var item in numbers)
            {
                if (int.TryParse(item.ToString(), out var digit))
                    result.Add(digit);
            }

            return result;
        }

        #endregion
    }
}