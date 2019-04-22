using System;
using System.Collections.Generic;

namespace Server.Models.Factories
{
    /// <summary>
    /// Produce DTO object from model class with dto model validation 
    /// </summary>
    /// <typeparam name="TDto">Model class</typeparam>
    /// <typeparam name="TFrom">Out DTO class</typeparam>
    public interface IDtoFactory<in TDto, out TFrom>
        where TDto : class
        where TFrom : class
    {
        /// <summary>
        /// Map domain model to DTO
        /// </summary>
        /// <param name="model">Domain model</param>
        /// <param name="validator">bool Func. Generally is TryModelValidation</param>
        /// <returns>DTO model</returns>
        TFrom Map(TDto model, Func<TFrom, bool> validator);

        /// <summary>
        /// Map domain model to DTO
        /// </summary>
        /// <param name="models">Domain models enumerator</param>
        /// <param name="validator">bool Func. Generally is TryModelValidation</param>
        /// <returns>DTO models enumerator</returns>
        IEnumerable<TFrom> Map(IEnumerable<TDto> models, Func<TFrom, bool> validator);
    }
}