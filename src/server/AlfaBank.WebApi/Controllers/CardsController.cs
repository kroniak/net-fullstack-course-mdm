using AlfaBank.Core.Data.Interfaces;
using AlfaBank.Core.Extensions;
using AlfaBank.Core.Infrastructure;
using AlfaBank.Core.Models;
using AlfaBank.Core.Models.Dto;
using AlfaBank.Core.Models.Factories;
using AlfaBank.Services.Checkers;
using AlfaBank.Services.Interfaces;
using AlfaBank.WebApi.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

// ReSharper disable PossibleMultipleEnumeration
namespace AlfaBank.WebApi.Controllers
{
    /// <inheritdoc />
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [BindProperties]
    public class CardsController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ICardRepository _cardRepository;
        private readonly ICardChecker _cardChecker;
        private readonly IDtoValidationService _dtoValidationService;
        private readonly IBankService _bankService;
        private readonly IDtoFactory<Card, CardGetDto> _dtoFactory;
        private readonly ILogger<CardsController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CardsController"/> class.
        /// </summary>
        /// <param name="dtoValidationService">Service for validate DTO</param>
        /// <param name="cardRepository">Cards Repository</param>
        /// <param name="userRepository">User Repository</param>
        /// <param name="cardChecker">Service fro card check</param>
        /// <param name="bankService">Main bank service</param>
        /// <param name="dtoFactory">Factory for DTO mapping</param>
        /// <param name="logger">Current Logger</param>
        [ExcludeFromCodeCoverage]
        public CardsController(
            IDtoValidationService dtoValidationService,
            ICardRepository cardRepository,
            IUserRepository userRepository,
            ICardChecker cardChecker,
            IBankService bankService,
            IDtoFactory<Card, CardGetDto> dtoFactory,
            ILogger<CardsController> logger)
        {
            _dtoValidationService = dtoValidationService ??
                                    throw new ArgumentNullException(nameof(dtoValidationService));
            _cardRepository = cardRepository ?? throw new ArgumentNullException(nameof(cardRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _cardChecker = cardChecker ?? throw new ArgumentNullException(nameof(cardChecker));
            _bankService = bankService ?? throw new ArgumentNullException(nameof(bankService));
            _dtoFactory = dtoFactory ?? throw new ArgumentNullException(nameof(dtoFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET api/cards

        /// <summary>
        /// Returns all cards for logged user
        /// </summary>
        /// <returns>A `CardGetDto` type</returns>
        /// <response code="200">Returns all cards</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CardGetDto>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<CardGetDto>> Get()
        {
            // Select
            var user = _userRepository.GetCurrentUser("admin@admin.ru");

            if (user == null)
            {
                return Forbid();
            }

            var cards = _cardRepository.GetAllWithTransactions(user);

            // Mapping
            var cardsDto = _dtoFactory.Map(cards, TryValidateModel);

            // Return
            return Ok(cardsDto);
        }

        // GET api/cards/5334343434343...

        /// <summary>
        /// Returns card by number for logged user
        /// </summary>
        /// <param name="number">Card number</param>
        /// <returns>A `CardGetDto` type</returns>
        /// <response code="200">Returns specified cards</response>
        /// <response code="400">If the item is invalid</response>
        [HttpGet("{number}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(CardGetDto), StatusCodes.Status200OK)]
        public ActionResult<CardGetDto> Get([CreditCard] string number)
        {
            // Validate
            if (!_cardChecker.CheckCardEmitter(number))
            {
                ModelState.AddModelError("number", "This card number is invalid");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogStateWarning("This card number is invalid.", ModelState);
                return BadRequest(ModelState);
            }

            // Select
            var user = _userRepository.GetCurrentUser("admin@admin.ru");

            if (user == null)
            {
                return Forbid();
            }

            var card = _cardRepository.GetWithTransactions(user, number, true);

            if (card == null)
            {
                _logger.LogWarning("This card number was not found. {number}", number);
                return NotFound();
            }

            // Mapping
            var dto = _dtoFactory.Map(card, TryValidateModel);

            // Validate
            if (dto != null) return Ok(dto);

            _logger.LogWarning("This card number was not found. {number}", number);
            return NotFound();
        }

        // POST api/cards

        /// <summary>
        /// Open new card for logged user and returns card info
        /// </summary>
        /// <param name="value">
        /// {
        ///    "name": "my_example_card",
        ///    "currency": 0,
        ///    "type": 1
        /// }
        /// </param>
        /// <returns>A `CardGetDto` type</returns>
        /// <response code="201">Returns new cards</response>
        /// <response code="400">If the item is invalid</response>
        [HttpPost]
        [ProducesResponseType(typeof(CardGetDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<CardGetDto> Post([FromBody] CardPostDto value)
        {
            // Validate
            var validateResult = _dtoValidationService.ValidateOpenCardDto(value);

            if (validateResult.HasErrors())
            {
                ModelState.AddErrors(validateResult);
            }

            if (!ModelState.IsValid)
            {
                _logger.LogStateWarning("This model is invalid.", ModelState);
                return BadRequest(ModelState);
            }

            // Select
            var user = _userRepository.GetCurrentUser("admin@admin.ru", false);

            if (user == null)
            {
                return Forbid();
            }

            var (card, openResult) = _bankService.TryOpenNewCard(
                user,
                value.Name,
                (Currency) value.Currency,
                (CardType) value.Type);

            if (openResult.HasErrors())
            {
                ModelState.AddErrors(openResult);
                _logger.LogStateError("Opening card was unsuccessfully.", ModelState);
                return BadRequest(ModelState);
            }

            // Mapping
            var dto = _dtoFactory.Map(card, TryValidateModel);

            switch (dto)
            {
                // Validate
                case null:
                    _logger.LogError("Opening card was unsuccessfully.");
                    return BadRequest("Не удалось выпустить карту");
                default:
                    return Created($"/api/cards/{dto.Number}", dto);
            }
        }

        // DELETE api/cards

        /// <summary>
        /// Card deleting is not accessible
        /// </summary>
        /// <returns>405 error</returns>
        /// <response code="405">All time</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        public IActionResult Delete() => StatusCode(405);

        // PUT api/cards

        /// <summary>
        /// Card updating is not accessible
        /// </summary>
        /// <returns>405 error</returns>
        /// <response code="405">All time</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        public IActionResult Put() => StatusCode(405);
    }
}