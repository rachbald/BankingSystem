using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DataAccess;
using Common;

namespace BL
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AccountTypesService" in code, svc and config file together.
    public class AccountTypesService : IAccountTypesService
    {
        public IEnumerable<AccountType> GetAccountTypes()
        {
            return new AccountTypeRepository().GetAccountTypes();
        }

        public AccountType GetAccountTypeById(int id)
        {
            return new AccountTypeRepository().GetAccountTypeById(id);
        }
    }
}
