using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.Models
{
    public class TokenValidationInput
    {
        public string UserName { get; set; }
        public string Token { get; set; }
    }
}
