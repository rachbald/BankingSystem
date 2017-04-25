using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineBankingSystem.Models
{
    public class LogsModel
    {
        public string Client { get; set; }
        public string Account { get; set; }
        public string ExternalAccount { get; set; }
        public String Date { get; set; }
        public String Details { get; set; }
        public string Amount { get; set; }
        public string Currency { get; set; }
        public int Type { get; set;}
    }
}