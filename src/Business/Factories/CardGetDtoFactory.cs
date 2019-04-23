using Models.Dto;
using Models;
using Business.Extensions;
using AutoMapper;

namespace Business.Factories
{
    /// <inheritdoc />
    public class CardGetDtoFactory : DtoFactory<Card, CardGetDto>
    {
        /// <inheritdoc />
        protected override CardGetDto Map(Card card)
		{
			Mapper.Reset();
			Mapper.Initialize(cfg => cfg.CreateMap<Card, CardGetDto>()
				.ForMember("Number", opt => opt.MapFrom(c => c.CardNumber))
				.ForMember("Exp", opt => opt.MapFrom(c => c.DtOpenCard.ToShortStringFormat(c.ValidityYear))));
			return Mapper.Map<Card, CardGetDto>(card);
		}
    }
}