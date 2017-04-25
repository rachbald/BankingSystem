using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineBankingSystem.Models
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Token { get; set; }

        public string Message { get; set; }
    }
}