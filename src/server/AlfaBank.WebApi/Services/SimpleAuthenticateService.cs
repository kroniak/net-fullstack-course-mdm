using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AlfaBank.Core.Data.Interfaces;
using AlfaBank.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AlfaBank.WebApi.Services
{
    /// <inheritdoc />
    public class SimpleAuthenticateService : ISimpleAuthenticateService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        /// <inheritdoc />
        public SimpleAuthenticateService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <inheritdoc />
        public string CheckUserCredentials(string userName, string password)
        {
            var user = _userRepository.GetSecureUser(userName);

            if (user == null) return null;

            var verificationResult = new PasswordHasher<User>().VerifyHashedPassword(user, user.Password, password);

            if (verificationResult != PasswordVerificationResult.Success) return null;

            // authentication successful. Generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = _configuration.GetSection("Auth").GetValue<string>("Key");

            var keyEncoded = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.UserName)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyEncoded),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var result = tokenHandler.WriteToken(token);
            return result;
        }
    }
}