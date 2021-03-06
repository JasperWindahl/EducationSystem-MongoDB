using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using WebApi.Authentication;
using WebApi.Helpers;
using WebApi.Models;
using static WebApi.Authentication.Hashing;
// ReSharper disable RedundantExplicitArrayCreation

namespace WebApi.Controllers
{
    [Route("api/auth")]
    [Authorize]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly DatabaseHelper _db;
        private readonly string _collection = "Users";

        public AuthenticationController(IConfiguration configuration)
        {
            _configuration = configuration;
            _db = new DatabaseHelper(_configuration);
        }

        /// <summary>
        /// Get token, authenticating user credentials
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [Route("token")]
        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateToken([FromBody] TokenRequest login)
        {
            IActionResult response = Unauthorized();
            var user = AuthenticateUser(login);
            if (user.Name != null)
            {
                var claims = new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim("Roles",user.Roles)
                };
                var tokenString = BuildToken(claims);
                response = Ok(tokenString);
            }
            return response;
        }

        /// <summary>
        /// Creates a new user in Database
        /// </summary>
        /// <param name="registerUser"></param>
        /// <returns></returns>
        [Route("register")]
        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateUser([FromBody] User registerUser)
        {
            IActionResult response = BadRequest();
            if (registerUser.Username == null || registerUser.Password == null) return response;
            if (CheckUserExists(registerUser.Username)) { return response; }

            var salt = GenerateSalt();
            var user = new User
            {
                Username = registerUser.Username,
                Password = GenerateHash(registerUser.Password, salt),
                Salt = salt,
                Email = registerUser.Email,
                FullName = registerUser.FullName,
                Roles = "auth.mongo"
            };
            _db.InsertDocument(_collection, user);
            response = Ok();
            return response;
        }

        /// <summary>
        /// Build token after successful credentials validation
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        private object BuildToken(Claim[] claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiryTime = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiryMinutes"]));
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                expires: expiryTime,
                claims: claims,
                signingCredentials: creds);
            var details = new
            {
                tokenString = new JwtSecurityTokenHandler().WriteToken(token),
                expiryDate = expiryTime.ToString("MM/dd/yyyy HH:mm:ss")
            };
            return details;
        }

        /// <summary>
        /// Authenticates user credentials
        /// </summary>
        /// <param name="tokenRequest"></param>
        /// <returns></returns>
        public TokenDetails AuthenticateUser(TokenRequest tokenRequest)
        {
            var tokenDetails = new TokenDetails();
            var users = GetUserByUsername(tokenRequest.Username);

            foreach (var user in users)
            {
                var hashedPassword = GenerateHash(tokenRequest.Password, user.Salt);
                if (user.Username != tokenRequest.Username || hashedPassword != user.Password) continue;
                tokenDetails.Email = user.Email;
                tokenDetails.Name = user.FullName;
                tokenDetails.Roles = user.Roles;
                break;
            }
            return tokenDetails;
        }

        /// <summary>
        /// Gets a given User from the database
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        private IEnumerable<User> GetUserByUsername(string userName)
        {
            var filter = MongoDB.Driver.Builders<User>.Filter.Eq("Username", userName);
            return _db.GetDocumentsByFilter(_collection, filter);
        }

        /// <summary>
        /// Check if a User with a given username exists in the database
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        private bool CheckUserExists(string userName)
        {
            var filter = MongoDB.Driver.Builders<User>.Filter.Eq("Username", userName);
            var result = _db.GetDocumentsByFilter(_collection, filter).Count();
            return result > 0;
        }
    }
}
