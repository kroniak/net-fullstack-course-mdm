using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Models;
using Server.Models.Dto;
using Server.Services;

namespace Server.Data
{
    public class CardRepository : ICardRepository
    {
        private readonly BlContext _context;

        public CardRepository(BlContext context)
        {
            _context = context;
        }
            public IQueryable<Card> GetDtoCards()
        {

            var crs = from b in _context.Cards
                      select b;

            return crs;
        }
        public Card GetCard(string cardNumber)
        {
            var crs = _context.Cards.Include(b => b.CardNumber).Select(b => new Card()
            {
                CardNumber = b.CardNumber,
                CardName = b.CardName,
                CardValidDate = b.CardValidDate
            }).SingleOrDefault(b => b.CardNumber == cardNumber);

            return crs;
        }

        public IEnumerable<Card> GetCards()
        {
        //https://docs.microsoft.com/ru-ru/aspnet/core/web-api/?view=aspnetcore-2.2
            return GetDtoCards();     
  }

        public bool PostCard(InputDataDtoCards cardPost)
        {
            Card card_db = new Card();
            card_db.CardName = cardPost.CardName;
            //card_db.CardNumber = cardPost.CardNumber;
            CreditCardNumberGeneratorService cardnum = new CreditCardNumberGeneratorService();
            card_db.CardNumber = cardnum.Generate();
            card_db.UserId = cardPost.UserId;

            // сформируем дату валидности
            card_db.CardValidDate = DateTime.Now.AddYears(2);
            try
            {
                //сохраним в БД
                _context.Cards.Add(card_db);
                _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
