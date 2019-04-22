using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Models.Factories
{
    /// <inheritdoc />
    public abstract class DtoFactory<TDto, TFrom> : IDtoFactory<TDto, TFrom>
        where TDto : class
        where TFrom : class
    {
        /// <inheritdoc />
        public TFrom Map(TDto model, Func<TFrom, bool> validator)
        {
            var dto = Map(model);
            return validator(dto) ? dto : null;
        }

        /// <inheritdoc />
        public IEnumerable<TFrom> Map(IEnumerable<TDto> models, Func<TFrom, bool> validator)
            => models.Select(Map).Where(validator);

        /// <summary>
        /// Abstract Map method for realization in derived classes
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Dto Model</returns>
        protected abstract TFrom Map(TDto model);
    }
}