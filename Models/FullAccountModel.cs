using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineBankingSystem.AccReference;
using System.Web.Mvc;

namespace OnlineBankingSystem.Models
{
    public class FullAccountModel
    {
        public FullAccountModel(string accountNo)
            : this()
        {
            myAccount = new AccountsServiceClient().GetAccountByNo(accountNo);
        }

        public Account myAccount { get; set; }
        public SelectList AccountType { get; set; }
        public SelectList FlexibleRate { get; set; }

        public FullAccountModel()
        {
            AccountType = new SelectList(new AccTypesReference.AccountTypesServiceClient().GetAccountTypes(), "Id", "Name");
            FlexibleRate = new SelectList( new FixedRatesReference.FixedRatesServiceClient().GetFixedRates(), "Id", "Months");
        }
    }
}