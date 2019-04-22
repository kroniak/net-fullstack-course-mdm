using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Server.Data.Interface;
using Server.Exceptions;
using Server.Models;
using Server.Models.DTO;
using Server.Services;
using Server.Services.Checkers;
using Server.Infrastructure;

namespace Server.Controllers
{
	[Route("api/[controller]")]
    public class CardsController : Controller
    {
        private readonly ICardChecker _cardChecker;
		private readonly ICardService _cardService;
		private readonly ICardRepository _cardRepository;

		public CardsController(
			ICardChecker cardChecker,
			ICardService cardService,
			ICardRepository cardRepository)
        {
            _cardChecker = cardChecker ??
                           throw new CriticalException(nameof(cardChecker));
			_cardService = cardService ??
						   throw new CriticalException(nameof(cardService));
			_cardRepository = cardRepository ??
						   throw new CriticalException(nameof(cardRepository));
		}

		// GET api/cards
		[HttpGet]
		public ActionResult<IEnumerable<CardDTO>> Get()
		{
			var cardList = _cardRepository.GetCards();
			Mapper.Initialize(cfg => cfg.CreateMap<IEnumerable<Card>, IEnumerable<CardDTO>>());
			var cardListDTO = new List<CardDTO>();
			foreach (var card in cardList)
			{
				cardListDTO.Add(Mapper.Map<Card, CardDTO>(card));
			}
			
			return  Ok(cardListDTO);
		}

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

			if (card == null)
			{
				return NotFound("Card with this number not found");
			}

			Mapper.Initialize(cfg => cfg.CreateMap<Card, CardDTO>());
			var cardDTO = Mapper.Map<Card, CardDTO>(card);

			return Ok(cardDTO);
        }

        // POST api/cards
        [HttpPost]
        public IActionResult Post([FromBody] string cardType)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var card = new Card
			{
				CardName = "test card",
				CardNumber = _cardService.GenerateNewCardNumber(_cardService.GetCardType(cardType))
			};

			Mapper.Initialize(cfg => cfg.CreateMap<Card, CardDTO>());
			var cardListDTO = Mapper.Map<Card, CardDTO>(card);

			if (card == null)
			{
				return NotFound("Card with this number not found");
			}

			return Ok(card);
		}

        // DELETE api/cards/5
        [HttpDelete("{number}")]
        public IActionResult Delete(string number) => StatusCode(405);

        //TODO PUT method
    }
}