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

        private static List<LoginInput> listOfUsers = new List<LoginInput>()
        {
            new LoginInput{ Username = "admin", Password = "password" },
            new LoginInput{ Username = "user", Password = "password" }
        };

        public LoginInput GetUserDetails(LoginInput loginInput)
        {
            LoginInput userDetails = listOfUsers.Find(userDetails => userDetails.Username == loginInput.Username && userDetails.Password == loginInput.Password);
            return userDetails;
        }


    }
}
