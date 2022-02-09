using Authorization.Models;
using Authorization.Repository;
using Authorization.Services;
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

        private static IConfiguration _config;
        private readonly IAuthenticationService service;
        public AuthenticationController(IConfiguration config, IAuthenticationService _service)
        {
            _config = config;
            service = _service;
        }


        [HttpPost("login")]
        public IActionResult Login(LoginInput loginInput)
        {
            IActionResult res = Unauthorized();
            var userId = service.AuthenticateUser(loginInput);
            if (userId == null)
            {
                return NotFound();
            }
            else
            {
                var JwtToken = service.GenerateJsonWebToken(userId);
                res = Ok(new { userId = userId, token = JwtToken });
                return res;
            }
        }

        [HttpGet("validate-token")]
        public bool ValidateUserToken()
        {
            Request.Headers.TryGetValue("Authorization", out var JwtToken);
            Request.Headers.TryGetValue("userName", out var UserName);

            string validatedUser = service.ValidationUser(UserName, JwtToken);
            if (validatedUser == null)
            {
                return false;
            }
            else if (validatedUser == "Successfully validated.")
            {
                return true;
            }
            else
            {
                return false;
            }

        }

    }
}
