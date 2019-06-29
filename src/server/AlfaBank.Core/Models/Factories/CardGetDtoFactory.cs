using System;
using System.Diagnostics.CodeAnalysis;
using AlfaBank.Core.Models.Dto;
using AutoMapper;

namespace AlfaBank.Core.Models.Factories
{
    /// <inheritdoc />
    public class CardGetDtoFactory : DtoFactory<Card, CardGetDto>
    {
        [ExcludeFromCodeCoverage]
        public CardGetDtoFactory(IMapper mapper)
        {
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <inheritdoc />
        protected override CardGetDto Map(Card card) => Mapper.Map<CardGetDto>(card);
    }
}