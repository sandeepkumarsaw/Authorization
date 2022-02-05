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
        /*public IActionResult Index()
        {
            return View();
        }*/
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
            //AuthenticationRepository repo= new AuthenticationRepository(_config);
            IActionResult res = Unauthorized();
            var userId = service.AuthenticateUser(loginInput);
            if(userId == null)
            {
                return NotFound();
            }
            else
            {
                var token = service.GenerateJsonWebToken(userId);
                res = Ok(new { userId = userId, token = token });
                return res;
            }
        }
         
        [HttpGet("validate-token")]
        public IActionResult ValidateUserToken()
        {
            Request.Headers.TryGetValue("Authorization", out var Token);
            Request.Headers.TryGetValue("userName", out var UserName);
            /*if (UserName == null)
            {
                return NotFound("userName is required");
            }*/
            string validated = service.ValidationUser(UserName, Token);
            if (validated == null)
            {
                return NotFound("userName and Token is required");
            }
            else if(validated== "Successfully validated.")
            {
                return Ok(validated);
            }
            else
            {
                return BadRequest(validated);
            }

        }

    }
}
