using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineBankingSystem.Models
{
    public class AccountModel
    {
        public int Id { get; set; }
        public string AccountNo { get; set; }
        public string Name { get; set; }
        
        public int TypeId { get; set; }
        public string Type { get; set; }

        public DateTime Creation { get; set; }
        public decimal Balance { get; set; }
        public int User { get; set; }

        public int RateId { get; set; }
        public string Rate { get; set; }
        public string Currency { get; set; }
    }
}