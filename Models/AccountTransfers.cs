using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineBankingSystem.Models
{
    public class AccountTransfers
    {
        public string CurrentAccount { get; set; }
        public string SecondaryAccount { get; set; }
        public string TransferAmount { get; set; }
        public string Currency { get; set; }
    }
}