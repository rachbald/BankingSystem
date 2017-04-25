using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace DataAccess
{
    public class AccountTypeRepository : DbConnection
    {
        public AccountTypeRepository()
            : base ()
        {}

        public IEnumerable<AccountType> GetAccountTypes()
        {
            return Entity.AccountTypes.AsEnumerable();
        }

        public AccountType GetAccountTypeById(int id)
        {
            return Entity.AccountTypes.SingleOrDefault(t => t.Id == id);
        }
    }
}
