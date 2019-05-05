using System.Diagnostics.CodeAnalysis;
using AlfaBank.Core.Extensions;
using AlfaBank.Core.Models.Dto;
using AutoMapper;

// ReSharper disable UnusedMember.Global

namespace AlfaBank.Core.Models
{
    [ExcludeFromCodeCoverage]
    public class DomainToDtoProfile : Profile
    {
        public DomainToDtoProfile()
        {
            CreateMap<Card, CardGetDto>()
                .ForMember(d => d.Number, o => o.MapFrom(s => s.CardNumber))
                .ForMember(d => d.Type, o => o.MapFrom(s => s.CardType))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.CardName))
                .ForMember(d => d.Currency, o => o.MapFrom(s => s.Currency))
                .ForMember(d => d.Balance, o => o.MapFrom(s => s.RoundBalance))
                .ForMember(d => d.Exp, o => o.MapFrom(s => s.DtOpenCard.ToShortStringFormat(s.ValidityYear)));

            CreateMap<Transaction, TransactionGetDto>()
                .ForMember(d => d.DateTime, o => o.MapFrom(s => s.DateTime))
                .ForMember(d => d.From, o => o.MapFrom(
                    s => s.Card.CardNumber == s.CardFromNumber
                        ? s.CardFromNumber
                        : s.CardFromNumber.CardNumberWatermark()))
                .ForMember(d => d.To, o => o.MapFrom(
                    s => s.Card.CardNumber == s.CardToNumber
                        ? s.CardToNumber
                        : s.CardToNumber.CardNumberWatermark()))
                .ForMember(d => d.Sum, o => o.MapFrom(s => s.Sum))
                .ForMember(d => d.IsCredit, o => o.MapFrom(s => s.IsCredit));
        }
    }
}