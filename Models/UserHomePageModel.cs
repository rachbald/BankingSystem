using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineBankingSystem.Models
{
    public class UserHomePageModel
    {
        public string AccountNo {get; set;}
        public string Funds { get; set; }

        public string Type { get; set; }
        public string Name { get; set; }
        public string Currency { get; set; }
    }
}