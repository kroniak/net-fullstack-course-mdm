using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Exceptions;
using Server.Models;
using Server.Models.Dto;
using Server.Services.Checkers;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : Controller
    {
        
        private readonly ICardChecker _cardChecker;
        private readonly ICardRepository _cardRepository;
        public CardsController(ICardRepository cardRepository,ICardChecker cardChecker )
        {
            _cardRepository = cardRepository;
            _cardChecker = cardChecker ??
                          throw new CriticalException(nameof(cardChecker));
            
        }



        
        


        // GET api/cards
        [HttpGet]
        //https://docs.microsoft.com/ru-ru/aspnet/core/web-api/?view=aspnetcore-2.2
        public async Task<ActionResult<IEnumerable<OtputDataDtoCards>>>  GetDtoCards()
        {
            var crs =   _cardRepository.GetCards();
            return Ok(crs);
        }

        // GET api/cards/5334343434343...
        [HttpGet("{number}")]
        public IActionResult GetCard(string number)
        {
            var crs = _cardRepository.GetCard(number);
            OtputDataDtoCards card_db = new OtputDataDtoCards();
            card_db.CardName = crs.CardName;
            card_db.CardNumber = crs.CardNumber;
            card_db.CardValidDate = crs.CardValidDate;
            return Ok(card_db);
        }

 

            // POST api/cards
        [HttpPost]
        //public async Task<ActionResult<Card>> PostCard(InputDataDtoCards card)
        //https://docs.microsoft.com/ru-ru/aspnet/core/web-api/action-return-types?view=aspnetcore-2.2
        public async Task<IActionResult> CreateAsync([FromBody] InputDataDtoCards card)
        {

            // проверки
            //InputDataDtoCards
            //string num = card.CardNumber;
            //if (string.IsNullOrWhiteSpace(num))
            //{
            //    ModelState.AddModelError(key: "CardNumber", errorMessage: "This card number is IsNullOrWhiteSpace");
            //}
            //// проверим строка может быть числом или это безмысленные символы
            //long n;
            //if (long.TryParse(num, out n) == false)
            //{
            //    ModelState.AddModelError(key: "CardNumber", errorMessage: "This card number is not number");
            //}
            //if (!_cardChecker.CheckCardNumber(num))
            //{
            //    ModelState.AddModelError(key: "CardNumber", errorMessage: "This card number is invalid");
            //}
            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);

            try
            {
                bool res =_cardRepository.PostCard(card);
                if (res == true) return Ok();
                else
                {
                    ModelState.AddModelError(key: "CardNumber", errorMessage: "Error , no insert card to DB. The card must be unique." );
                    return BadRequest(ModelState);
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(key: "CardNumber", errorMessage: "Error , no insert card to DB. The card must be unique." + ex.Message);
                return BadRequest(ModelState);
            }

        }

        // DELETE api/cards/5
        [HttpDelete("{number}")]
        public IActionResult Delete(string number) => StatusCode(405);

        //TODO PUT method
    }
}