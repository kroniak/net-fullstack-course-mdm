using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using AlfaBank.Core.Data.Interfaces;
using AlfaBank.Core.Extensions;
using AlfaBank.Core.Infrastructure;
using AlfaBank.Core.Models;
using AlfaBank.Core.Models.Dto;
using AlfaBank.Core.Models.Factories;
using AlfaBank.Services.Checkers;
using AlfaBank.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable PossibleMultipleEnumeration
namespace AlfaBank.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [BindProperties]
    public class CardsController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ICardRepository _cardRepository;
        private readonly ICardChecker _cardChecker;
        private readonly IDtoValidationService _dtoValidationService;
        private readonly IBankService _bankService;
        private readonly IDtoFactory<Card, CardGetDto> _dtoFactory;

        [ExcludeFromCodeCoverage]
        public CardsController(IDtoValidationService dtoValidationService,
            ICardRepository cardRepository,
            IUserRepository userRepository,
            ICardChecker cardChecker, IBankService bankService,
            IDtoFactory<Card, CardGetDto> dtoFactory)
        {
            _dtoValidationService = dtoValidationService ??
                                    throw new ArgumentNullException(nameof(dtoValidationService));
            _cardRepository = cardRepository ?? throw new ArgumentNullException(nameof(cardRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _cardChecker = cardChecker ?? throw new ArgumentNullException(nameof(cardChecker));
            _bankService = bankService ?? throw new ArgumentNullException(nameof(bankService));
            _dtoFactory = dtoFactory ?? throw new ArgumentNullException(nameof(dtoFactory));
        }

        // GET api/cards
        [HttpGet]
        public ActionResult<IEnumerable<CardGetDto>> Get()
        {
            // Select
            var cards = _cardRepository.All(_userRepository.GetCurrentUser());

            // Mapping
            var cardsDto = _dtoFactory.Map(cards, TryValidateModel);

            // Return
            return Ok(cardsDto);
        }

        // GET api/cards/5334343434343...
        [HttpGet("{number}")]
        public ActionResult<CardGetDto> Get([CreditCard] string number)
        {
            // Validate
            if (!_cardChecker.CheckCardEmitter(number))
                ModelState.AddModelError("number", "This card number is invalid");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Select
            var card = _cardRepository.Get(_userRepository.GetCurrentUser(), number);

            // Mapping
            var dto = _dtoFactory.Map(card, TryValidateModel);

            // Validate
            if (dto == null) return NotFound();

            return Ok(dto);
        }

        // POST api/cards
        [HttpPost]
        public ActionResult<CardGetDto> Post([FromBody] CardPostDto value)
        {
            // Validate
            var validateResult = _dtoValidationService.ValidateOpenCardDto(value);
            if (validateResult.HasErrors()) ModelState.AddErrors(validateResult);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Select
            var (card, openResult) = _bankService.TryOpenNewCard(
                _userRepository.GetCurrentUser(),
                value.Name,
                (Currency) value.Currency,
                (CardType) value.Type);

            if (openResult.HasErrors())
            {
                ModelState.AddErrors(openResult);
                return BadRequest(ModelState);
            }

            // Mapping
            var dto = _dtoFactory.Map(card, TryValidateModel);

            // Validate
            if (dto == null) return BadRequest("Не удалось выпустить карту");

            return Created($"/api/cards/{dto.Number}", dto);
        }

        // DELETE api/cards
        [HttpDelete]
        public IActionResult Delete() => StatusCode(405);

        // PUT api/cards
        [HttpPut]
        public IActionResult Put() => StatusCode(405);
    }
}