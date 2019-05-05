using System;
using System.Collections.Generic;
using System.Linq;
using AlfaBank.Core.Exceptions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AlfaBank.Core.Extensions
{
    public static class ValidationExtensions
    {
        /// <summary>
        /// Add error to custom ModelErrorsSet
        /// </summary>
        /// <param name="list">List to add</param>
        /// <param name="condition">Condition to check</param>
        /// <param name="field">Name of the field</param>
        /// <param name="message">Message of the error in en</param>
        /// <param name="localizedMessage">Message of the error in ru</param>
        /// <param name="type">Type of the error</param>
        public static ICollection<CustomModelError> AddError(
            this ICollection<CustomModelError> list,
            Func<bool> condition,
            string field,
            string message,
            string localizedMessage,
            TypeCriticalException type = TypeCriticalException.TRANSACTION) =>
            condition() ? list.AddError(field, message, localizedMessage, type) : list;

        /// <summary>
        /// Add error to custom ModelErrorsSet
        /// </summary>
        /// <param name="list">List to add</param>
        /// <param name="field">Name of the field</param>
        /// <param name="message">Message of the error in en</param>
        /// <param name="localizedMessage">Message of the error in ru</param>
        /// <param name="type">Type of the error</param>
        public static ICollection<CustomModelError> AddError(
            this ICollection<CustomModelError> list,
            string field,
            string message,
            string localizedMessage,
            TypeCriticalException type = TypeCriticalException.TRANSACTION)
        {
            list.Add(new CustomModelError
            {
                FieldName = field,
                Message = message,
                LocalizedMessage = localizedMessage ?? message,
                Type = type
            });

            return list;
        }

        /// <summary>
        /// Add CustomModelErrors to ModelStateDictionary
        /// </summary>
        /// <param name="dict">Dictionary</param>
        /// <param name="errors">List of the errors</param>
        public static void AddErrors(this ModelStateDictionary dict,
            IEnumerable<CustomModelError> errors)
        {
            foreach (var error in errors)
                dict.AddModelError(error.FieldName, error.LocalizedMessage ?? error.Message ?? "Field state is wrong");
        }

        public static bool HasErrors(this IEnumerable<CustomModelError> list) =>
            list.Any();
    }
}