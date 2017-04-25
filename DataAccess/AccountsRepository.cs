using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace DataAccess
{
    public class AccountsRepository : DbConnection
    {
        public AccountsRepository()
            : base()
        {
        }

        public IEnumerable<Account> GetAccounts()
        {
            return Entity.Accounts.AsEnumerable<Account>();
        }

        public IEnumerable<Account> GetAccounts1()
        {
            return Entity.Accounts.AsEnumerable<Account>();
        }

        public IEnumerable<Account> GetUserAccounts(string username)
        {
            return Entity.Accounts.Where(a => a.User.Equals(username));
        }

        public IEnumerable<Account> GetOtherAccounts(string username)
        {
            return Entity.Accounts.Where(a => !a.User.Equals(username));
        }

        public Account GetAccountByNo(string accountNo)
        {
            return Entity.Accounts.SingleOrDefault(a => a.AccountNo.Equals(accountNo));
        }

        public Account GetAccountById(int id)
        {
            return Entity.Accounts.SingleOrDefault(a => a.Id.Equals(id));
        }

        public string AddUserAccount(Account a)
        {
            try
            {
                Entity.AddToAccounts(a);
                Entity.SaveChanges();
                              

                string id = a.AccountNo;

                return id;
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public bool UpdateAccount(Account a)
        {
            try
            {

                Entity.Accounts.Attach(GetAccountByNo(a.AccountNo));
                Entity.Accounts.ApplyCurrentValues(a);
                Entity.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }
        
        public bool DeleteAccount(string accountNo)
        {
            try
            {
                Account a = GetAccountByNo(accountNo);
                
                Entity.DeleteObject(a);
                Entity.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }        
    }
}
  