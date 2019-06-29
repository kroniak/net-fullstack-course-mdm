using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using AlfaBank.Core.Models.Dto;
using AlfaBank.WebApi.Middleware;
using AlfaBank.WebApi.Services;
using Microsoft.AspNetCore.Authorization;

// ReSharper disable PossibleMultipleEnumeration
namespace AlfaBank.WebApi.Controllers
{
    /// <inheritdoc />
    [ApiController]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [BindProperties]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly ISimpleAuthenticateService _authenticateService;
        private readonly ILogger<AuthController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CardsController"/> class.
        /// </summary>
        /// <param name="authenticateService"></param>
        /// <param name="logger">Current Logger</param>
        [ExcludeFromCodeCoverage]
        public AuthController(
            ISimpleAuthenticateService authenticateService,
            ILogger<AuthController> logger)
        {
            _authenticateService = authenticateService ?? throw new ArgumentNullException(nameof(authenticateService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // POST /auth/login
        /// <summary>
        /// Login user
        /// </summary>
        /// <returns>A `string` type with token</returns>
        /// <response code="200">Login user successfully</response>
        [Route("/auth/login")]
        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        [AllowAnonymous]
        public ActionResult<string> Login([FromBody] UserDto userDto)
        {
            // try to validate user model
            if (!ModelState.IsValid)
            {
                _logger.LogStateWarning("This login model is invalid.", ModelState);
                return BadRequest(ModelState);
            }

            // try to verify user data
            var token = _authenticateService.CheckUserCredentials(userDto.Username, userDto.Password);

            if (token == null)
            {
                _logger.LogWarning("Authenticating is failed.");
                return Unauthorized();
            }

            // Return
            return Ok(new {token});
        }

        // POST /auth/login
        /// <summary>
        /// Verify user JWT
        /// </summary>
        /// <response code="200">Login user successfully</response>
        [Route("/auth/verify")]
        [HttpGet]
        [ProducesResponseType(typeof(ActionResult), StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public ActionResult Verify()
        {
            // Return
            return Ok(new {userName = User.Identity.Name});
        }
    }
}