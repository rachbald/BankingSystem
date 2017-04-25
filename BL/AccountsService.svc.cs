using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Common;
using DataAccess;

namespace BL
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AccountsService" in code, svc and config file together.
    public class AccountsService : IAccountsService
    {
        

        public IEnumerable<Account> GetAccounts()
        {
            return new AccountsRepository().GetAccounts();
        }

        public IEnumerable<Account> GetAccounts1()
        {
            return new AccountsRepository().GetAccounts1();
        }

        public IEnumerable<Account> GetOtherAccounts(string username)
        {
            return new AccountsRepository().GetOtherAccounts(username);
        }
        
        public Account GetAccountByNo(string accountNo)
        {
            return new AccountsRepository().GetAccountByNo(accountNo);
        }

        public Account GetAccountById(int id)
        {
            return new AccountsRepository().GetAccountById(id);
        }

        public IEnumerable<Account> GetUserAccounts(string username)
        {
            return new AccountsRepository().GetUserAccounts(username);
        }

        public string AddUserAccount(Account a)
        {
            return new AccountsRepository().AddUserAccount(a);
        }

        public bool UpdateAccount(Account a)
        {
            return new AccountsRepository().UpdateAccount(a);
        }

        public bool DeleteAccount(string accountNo)
        {
            return new AccountsRepository().DeleteAccount(accountNo);
        }

        
    }
}
