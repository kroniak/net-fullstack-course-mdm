using System;
using System.Diagnostics.CodeAnalysis;
using AlfaBank.Core.Exceptions;
using AlfaBank.Core.Models.Dto;
using AutoMapper;

namespace AlfaBank.Core.Models.Factories
{
    /// <inheritdoc />
    public class TransactionGetDtoFactory : DtoFactory<Transaction, TransactionGetDto>
    {
        [ExcludeFromCodeCoverage]
        public TransactionGetDtoFactory(IMapper mapper)
        {
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <inheritdoc />
        protected override TransactionGetDto Map(Transaction model)
        {
            if (model.Card == null)
                throw new CriticalException("internal error", TypeCriticalException.TRANSACTION);

            return Mapper.Map<TransactionGetDto>(model);
        }
    }
}