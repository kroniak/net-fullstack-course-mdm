using AlfaBank.Core.Data.Interfaces;
using AlfaBank.Core.Extensions;
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
using Microsoft.AspNetCore.Authorization;

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
    [Authorize]
    public class TransactionsController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IBankService _bankService;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICardChecker _cardChecker;
        private readonly IDtoValidationService _dtoValidationService;
        private readonly IDtoFactory<Transaction, TransactionGetDto> _dtoFactory;
        private readonly ILogger<TransactionsController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionsController"/> class.
        /// </summary>
        /// <param name="dtoValidationService">Service for validate DTO</param>
        /// <param name="transactionRepository">Main Transaction repository</param>
        /// <param name="userRepository">User Repository</param>
        /// <param name="cardChecker">Service fro card check</param>
        /// <param name="bankService">Main bank service</param>
        /// <param name="dtoFactory">Factory for DTO mapping</param>
        /// <param name="logger">Current logger</param>
        [ExcludeFromCodeCoverage]
        public TransactionsController(
            IDtoValidationService dtoValidationService,
            IUserRepository userRepository,
            ITransactionRepository transactionRepository,
            ICardChecker cardChecker,
            IBankService bankService,
            IDtoFactory<Transaction, TransactionGetDto> dtoFactory,
            ILogger<TransactionsController> logger)
        {
            _dtoValidationService = dtoValidationService ??
                                    throw new ArgumentNullException(nameof(dtoValidationService));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _transactionRepository =
                transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
            _cardChecker = cardChecker ?? throw new ArgumentNullException(nameof(cardChecker));
            _bankService = bankService ?? throw new ArgumentNullException(nameof(bankService));
            _dtoFactory = dtoFactory ?? throw new ArgumentNullException(nameof(dtoFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET api/transactions/5334343434343?skip=...

        /// <summary>
        /// Returns transactions by card number for logged user
        /// </summary>
        /// <param name="number">Card number</param>
        /// <param name="skip">How many transaction to skip</param>
        /// <returns>A `TransactionGetDto` type</returns>
        /// <response code="200">Returns cards transactions</response>
        /// <response code="400">If the item is invalid</response>
        [HttpGet("{number}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IEnumerable<TransactionGetDto>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<TransactionGetDto>> Get(
            [Required] [CreditCard] string number,
            [FromQuery] [Range(1, 1000)] int skip = 0)
        {
            // Validate
            if (!_cardChecker.CheckCardEmitter(number))
            {
                ModelState.AddModelError("number", "This card number is invalid");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogStateWarning("This model is invalid.", ModelState);
                return BadRequest(ModelState);
            }

            // Select
            var user = _userRepository.GetUser(User.Identity.Name);

            if (user == null)
            {
                return Forbid();
            }

            var transactions = _transactionRepository.Get(
                user,
                number,
                skip,
                10);

            // Mapping
            var transactionsDto = _dtoFactory.Map(transactions, TryValidateModel);

            // Return
            return Ok(transactionsDto);
        }

        // POST api/transactions

        /// <summary>
        /// Transfer money from card to card for logged user and returns transaction info
        /// </summary>
        /// <param name="value">
        /// {
        ///    "from":"6271190065239038",
        ///    "to": "4083966651806881",
        ///    "sum": 0.1
        /// }
        /// </param>
        /// <returns>A `TransactionGetDto` type</returns>
        /// <response code="201">Returns new transaction</response>
        /// <response code="400">If the item is invalid</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(TransactionGetDto), StatusCodes.Status201Created)]
        public ActionResult<TransactionGetDto> Post([FromBody] [Required] TransactionPostDto value)
        {
            // Validate
            var validateResult = _dtoValidationService.ValidateTransferDto(value);
            if (validateResult.HasErrors())
            {
                ModelState.AddErrors(validateResult);
            }

            if (!ModelState.IsValid)
            {
                _logger.LogStateWarning("This model is invalid.", ModelState);
                return BadRequest(ModelState);
            }

            // Work
            var user = _userRepository.GetUser(User.Identity.Name);

            if (user == null)
            {
                return Forbid();
            }

            var (transaction, transferResult) = _bankService.TryTransferMoney(
                user,
                value.Sum,
                value.From,
                value.To);

            if (transferResult.HasErrors())
            {
                ModelState.AddErrors(transferResult);
                _logger.LogStateError("Transferring was unsuccessfully.", ModelState);

                return BadRequest(ModelState);
            }

            var cardFromNumber = value.From.ToNormalizedCardNumber();

            var dto = _dtoFactory.Map(transaction, TryValidateModel);

            switch (dto)
            {
                // Validate
                case null:
                    _logger.LogError("Transferring was unsuccessfully.");
                    return BadRequest("Transferring error");
                default:
                    return Created($"/transactions/{cardFromNumber}", dto);
            }
        }

        // DELETE api/transactions

        /// <summary>
        /// Transaction deleting is not accessible
        /// </summary>
        /// <returns>405 error</returns>
        /// <response code="405">All time</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        public IActionResult Delete() => StatusCode(405);

        // PUT api/transactions

        /// <summary>
        /// Transaction updating is not accessible
        /// </summary>
        /// <returns>405 error</returns>
        /// <response code="405">All time</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        public IActionResult Put() => StatusCode(405);
    }
}