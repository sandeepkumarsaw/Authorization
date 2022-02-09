using Authorization.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.Services
{
    public interface IAuthenticationService
    {
        public string AuthenticateUser(LoginInput loginInput);
        public string GenerateJsonWebToken(string userId);
        public string ValidationUser(string UserName, string JwtToken);
    }
}
