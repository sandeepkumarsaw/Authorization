using Authorization.Models;
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
    public class AuthenticationRepository: IAuthenticationRepository
    {

        private static List<LoginInput> list = new List<LoginInput>()
        {
            new LoginInput{ Username = "admin", Password = "password" },
            new LoginInput{ Username = "user", Password = "password" }
        };

        public LoginInput GetUserDetails(LoginInput login)
        {
            List<LoginInput> l = list;
            LoginInput cred = list.Find(user => user.Username == login.Username && user.Password == login.Password);
            return cred;
        }


    }
}
