using Authorization.Models;
using Authorization.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        /*public IActionResult Index()
        {
            return View();
        }*/
        private static IConfiguration _config;


        public AuthenticationController(IConfiguration config)
        {
            _config = config;
        }



        [HttpPost("login")]
        public IActionResult Login(LoginInput loginInput)
        {
            AuthenticationRepository repo= new AuthenticationRepository(_config);
            IActionResult res = Unauthorized();
            var userId = repo.AuthenticateUser(loginInput);
            if(userId == null)
            {
                return NotFound();
            }
            else
            {
                var token = repo.GenerateJsonWebToken(userId);
                res = Ok(new { userId = userId, token = token });
                return res;
            }
        }
         
        [HttpGet("validate-token")]
        public IActionResult ValidateUserToken()
        {
            Request.Headers.TryGetValue("Authorization", out var Token);
            Request.Headers.TryGetValue("userName", out var UserName);

            var tokenHandler = new JwtSecurityTokenHandler();
            AuthenticationRepository repo = new AuthenticationRepository(_config);
            var validationParameters = repo.GetValidationParameters(UserName);
            if (validationParameters == null)
            {
                return NotFound("userName is required");
            }
            SecurityToken validatedToken;

            try
            {
                IPrincipal principal = tokenHandler.ValidateToken(Token, validationParameters, out validatedToken);
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                    if (result == false)
                    {
                        return NotFound("Token is Invalid.");
                    }
                    else
                    {
                        if(validatedToken.ValidTo.ToFileTimeUtc() > DateTime.Now.ToFileTimeUtc())
                        {
                            return Ok("Successfully validated.");
                        } 
                        else
                        {
                            NotFound("Token expired.");
                        }
                    }
                }
                return NotFound("Token does not match or may expired.");
            }
            catch (Exception ex)
            {
                return NotFound("Token does not match or may expired.");
            }


        }

    }
}
