using Authorization.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.Provider
{
    public class UserProvider 
    {
        private static List<LoginInput> list = new List<LoginInput>()
        {
            new LoginInput{ Username = "admin", Password = "password" },
            new LoginInput{ Username = "user", Password = "password" }
        };

        public LoginInput GetUserLogin(LoginInput login)
        {
            List<LoginInput> l = list;
            LoginInput cred = list.FirstOrDefault(user => user.Username == login.Username && user.Password == login.Password);
            return cred;
        }

    }
}
