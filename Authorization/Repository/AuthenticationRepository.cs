using Authorization.Models;
using Authorization.Provider;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Repository
{
    public class AuthenticationRepository
    {
        private IConfiguration _config;
        public AuthenticationRepository(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateJsonWebToken(string userId)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: userId,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: cred
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string AuthenticateUser(LoginInput loginInput)
        {
            if (loginInput == null)
            {
                return null;
            }
            UserProvider up = new UserProvider();
            LoginInput user = up.GetUserLogin(loginInput);

            return user.Username;
        }

        public TokenValidationParameters GetValidationParameters(string UserName)
        {
            if (UserName == null)
            {
                return null;
            }
            else
            {
                TokenValidationParameter tokenParameter = new TokenValidationParameter(_config);
                return tokenParameter.GetParameters(UserName);
            }
        }

    }
}
