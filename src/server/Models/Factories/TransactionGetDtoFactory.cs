using Server.Exceptions;
using Server.Infrastructure;
using Server.Models.Dto;
using Server.Services.Extensions;

namespace Server.Models.Factories
{
    /// <inheritdoc />
    public class TransactionGetDtoFactory : DtoFactory<Transaction, TransactionGetDto>
    {
        /// <inheritdoc />
        protected override TransactionGetDto Map(Transaction model)
        {
            if (model.Card == null)
                throw new CriticalException("internal error", TypeCriticalException.TRANSACTION);

            return new TransactionGetDto
            {
                DateTime = model.DateTime,
                From = model.Card.CardNumber == model.CardFromNumber
                    ? model.CardFromNumber
                    : model.CardFromNumber.CardNumberWatermark(),
                To = model.Card.CardNumber == model.CardToNumber
                    ? model.CardToNumber
                    : model.CardToNumber.CardNumberWatermark(),
                Sum = model.Sum,
                IsCredit = model.IsCredit
            };
        }
    }
}