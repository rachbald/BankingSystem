using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineBankingSystem.Models
{
    public class FixedAccountModel
    {
        public int Id { get; set; }
        public string AccountNo { get; set; }
        public decimal Balance { get; set; }
        public string Duration { get; set; }
        public decimal Rate { get; set;}
        public decimal Interest { get; set; }
        public string TransferAccount { get; set; }
        public decimal TaxReduced { get; set; }
        public decimal NewBalance { get; set; }
    }
}