using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Server.Data.Interfaces;
using Server.Infrastructure;
using Server.Models;
using Server.Models.Dto;
using Server.Models.Factories;
using Server.Services.Checkers;
using Server.Services.Extensions;
using Server.Services.Interfaces;

// ReSharper disable PossibleMultipleEnumeration
namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [BindProperties]
    public class TransactionsController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IBankService _bankService;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICardChecker _cardChecker;
        private readonly IDtoValidationService _dtoValidationService;
        private readonly IDtoFactory<Transaction, TransactionGetDto> _dtoFactory;

        [ExcludeFromCodeCoverage]
        public TransactionsController(
            IDtoValidationService dtoValidationService,
            IUserRepository userRepository,
            ITransactionRepository transactionRepository,
            ICardChecker cardChecker,
            IBankService bankService,
            IDtoFactory<Transaction, TransactionGetDto> dtoFactory)
        {
            _dtoValidationService = dtoValidationService ??
                                    throw new ArgumentNullException(nameof(dtoValidationService));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _transactionRepository =
                transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
            _cardChecker = cardChecker ?? throw new ArgumentNullException(nameof(cardChecker));
            _bankService = bankService ?? throw new ArgumentNullException(nameof(bankService));
            _dtoFactory = dtoFactory ?? throw new ArgumentNullException(nameof(dtoFactory));
        }

        // GET api/transactions/5334343434343?skip=...
        [HttpGet("{number}")]
        public ActionResult<IEnumerable<TransactionGetDto>> Get(
            [Required] [CreditCard] string number, [FromQuery] [Range(1, 1000)] int skip = 0)
        {
            // Validate
            if (!_cardChecker.CheckCardEmitter(number))
                ModelState.AddModelError("number", "This card number is invalid");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Select 
            var transactions = _transactionRepository.GetTransactions(
                _userRepository.GetCurrentUser(),
                number,
                skip,
                10);

            // Mapping
            var transactionsDto = _dtoFactory.Map(transactions, TryValidateModel);

            // Return
            return Ok(transactionsDto);
        }

        // POST api/transactions
        [HttpPost]
        public ActionResult<TransactionGetDto> Post([FromBody] [Required] TransactionPostDto value)
        {
            // Validate
            var validateResult = _dtoValidationService.ValidateTransferDto(value);
            if (validateResult.HasErrors()) ModelState.AddErrors(validateResult);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Work
            var (transaction, transferResult) = _bankService.TryTransferMoney(
                _userRepository.GetCurrentUser(),
                value.Sum,
                value.From,
                value.To);

            if (transferResult.HasErrors())
            {
                ModelState.AddErrors(transferResult);
                return BadRequest(ModelState);
            }

            var cardFromNumber = value.From.ToNormalizedCardNumber();

            var dto = _dtoFactory.Map(transaction, TryValidateModel);

            // Validate
            if (dto == null) return BadRequest("Transferring error");

            return Created($"/transactions/{cardFromNumber}", dto);
        }

        // DELETE api/transactions
        [HttpDelete]
        public IActionResult Delete() => StatusCode(405);

        // PUT api/transactions
        [HttpPut]
        public IActionResult Put() => StatusCode(405);
    }
}