using AlfaBank.Core.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
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
    [BindProperties]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AuthController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CardsController"/> class.
        /// </summary>
        /// <param name="userRepository">User Repository</param>
        /// <param name="logger">Current Logger</param>
        [ExcludeFromCodeCoverage]
        public AuthController(
            IUserRepository userRepository,
            ILogger<AuthController> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET api/auth/login
        /// <summary>
        /// Login user
        /// </summary>
        /// <returns>A `string` type with token</returns>
        /// <response code="200">Login user successfully</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public ActionResult<string> Login()
        {
            // try to validate user model

            // try to verify user data

            // generate token

            // Return
            return Ok();
        }

        // GET api/auth/login
        /// <summary>
        /// Logout user
        /// </summary>
        /// <response code="200">Logout user successfully</response>
        [HttpPost("logout")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult Logout()
        {
            // try to validate model

            // try to verify user data

            // delete token

            // Return
            return Ok();
        }
    }
}