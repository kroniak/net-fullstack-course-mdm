using Models.Exceptions;
using Models.Infrastructure;
using Models.Dto;
using Models;
using Business.Extensions;
using AutoMapper;

namespace Business.Factories
{
    /// <inheritdoc />
    public class TransactionGetDtoFactory : DtoFactory<Transaction, TransactionGetDto>
    {
        /// <inheritdoc />
        protected override TransactionGetDto Map(Transaction model)
        {
            if (model.Card == null)
                throw new CriticalException("internal error", TypeCriticalException.TRANSACTION);

			Mapper.Reset();
			Mapper.Initialize(cfg => cfg.CreateMap<Transaction, TransactionGetDto>()
				.ForMember("From", opt => opt.MapFrom(t =>
					t.Card.CardNumber == t.CardFromNumber
					? t.CardFromNumber
					: t.CardFromNumber.CardNumberWatermark()))
				.ForMember("To", opt => opt.MapFrom(t =>
					t.Card.CardNumber == t.CardToNumber
					? t.CardToNumber
					: t.CardToNumber.CardNumberWatermark())));
			return Mapper.Map<Transaction, TransactionGetDto>(model);
        }
    }
}