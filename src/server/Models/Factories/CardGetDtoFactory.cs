using Server.Models.Dto;
using Server.Services.Extensions;

namespace Server.Models.Factories
{
    /// <inheritdoc />
    public class CardGetDtoFactory : DtoFactory<Card, CardGetDto>
    {
        /// <inheritdoc />
        protected override CardGetDto Map(Card card) =>
            new CardGetDto
            {
                Number = card.CardNumber,
                Type = (int) card.CardType,
                Name = card.CardName,
                Currency = (int) card.Currency,
                Exp = card.DtOpenCard.ToShortStringFormat(card.ValidityYear),
                Balance = card.RoundBalance
            };
    }
}