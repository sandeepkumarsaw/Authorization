using Authorization.Models;
using Authorization.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Services
{
    public class AuthenticationService: IAuthenticationService
    {
        private IConfiguration _config;
        private IAuthenticationRepository repository;
        public AuthenticationService(IConfiguration config, IAuthenticationRepository repo)
        {
            _config = config;
            repository = repo;
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
            //UserProvider up = new UserProvider();
            LoginInput user = repository.GetUserDetails(loginInput);
            if (user == null)
            {
                return null;
            }
            return user.Username;
        }
        public string ValidationUser(string UserName, string Token)
        {
            if (UserName == null || Token==null)
            {
                return null;
            }
            else
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                //TokenValidationParameter tokenParameter = new TokenValidationParameter(_config);
                var validationParameters = GetParameters(UserName);
                SecurityToken validatedToken;

                try
                {
                    IPrincipal principal = tokenHandler.ValidateToken(Token, validationParameters, out validatedToken);
                    if (validatedToken is JwtSecurityToken jwtSecurityToken)
                    {
                        var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                        if (result == false)
                        {
                            return "Token is Invalid.";
                        }
                        else
                        {
                            if (validatedToken.ValidTo.ToFileTimeUtc() > DateTime.Now.ToFileTimeUtc())
                            {
                                return "Successfully validated.";
                            }
                            else
                            {
                                return "Token expired.";
                            }
                        }
                    }
                    return "Token does not match or may expired.";
                }
                catch (Exception ex)
                {
                    return "Token does not match or may expired." + ex;
                }
            }


        }

        public TokenValidationParameters GetParameters(string userName)
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidIssuer = _config["Jwt:Issuer"],
                ValidAudience = userName,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]))
            };
        }
    }
}
