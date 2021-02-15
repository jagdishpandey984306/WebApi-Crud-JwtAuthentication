using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Shared.Model.TokenRequest;

namespace webapi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        IConfiguration _config;
        public TokenController(IConfiguration configuration)
        {
            _config = configuration;
        }

        // Generate a Token with expiration date and Claim meta-data.
        // And sign the token with the SIGNING_KEY
        private string GenerateJWT()
        {
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var expiry = DateTime.Now.AddMonths(6);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(issuer: issuer, audience: audience, notBefore: DateTime.Now, expires: expiry, signingCredentials: credentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            var stringToken = tokenHandler.WriteToken(token);
            return stringToken;
        }


        private bool ValidateUser(TokenRequest Request)
        {
            return Request.username == "webapi" && Request.password == "api1234" ? true : false;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] TokenRequest loginDetails)
        {
            if (loginDetails != null)
            {
                if (ValidateUser(loginDetails))
                {
                    var tokenString = GenerateJWT();
                    return Ok(tokenString);
                }
                else
                {
                    return Unauthorized();
                }
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
