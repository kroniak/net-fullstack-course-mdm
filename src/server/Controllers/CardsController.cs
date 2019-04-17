using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Server.Exceptions;
using Server.Models;
using Server.Models.DTO;
using Server.Services;
using Server.Services.Checkers;

namespace Server.Controllers
{
	[Route("api/[controller]")]
    public class CardsController : Controller
    {
        private readonly ICardChecker _cardChecker;

        public CardsController(ICardChecker cardChecker)
        {
            _cardChecker = cardChecker ??
                           throw new CriticalException(nameof(cardChecker));
        }

		// GET api/cards
		[HttpGet]
		public ActionResult<IEnumerable<CardDTO>> Get()
			=> Ok(UserSingleton.User.Cards);

		// GET api/cards/5334343434343...
		[HttpGet("{number}")]
        public ActionResult<CardDTO> Get(string number)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_cardChecker.CheckCardEmitter(number))
                return BadRequest("This card number is invalid");

            var card = new Card
            {
                CardName = "test card",
                CardNumber = number
            };

            if (card == null) return NotFound("Card with this number not found");

            return Ok(card);
        }

        // POST api/cards
        [HttpPost]
        public IActionResult Post([FromBody] string cardType) =>
            throw new NotImplementedException();

        // DELETE api/cards/5
        [HttpDelete("{number}")]
        public IActionResult Delete(string number) => StatusCode(405);

        //TODO PUT method
    }
}