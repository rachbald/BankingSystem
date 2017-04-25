using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineBankingSystem.TransReference;

namespace OnlineBankingSystem.Models
{
    public class TransactionsModel
    {
        public String Date { get; set; }
        public String Details { get; set; }
        public string Currency { get; set; }
        public String Amount { get; set; }        
    }
}