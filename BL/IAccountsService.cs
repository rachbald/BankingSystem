using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Common;

namespace BL
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAccountsService" in both code and config file together.
    [ServiceContract]
    public interface IAccountsService
    {
        [OperationContract]
        IEnumerable<Account> GetOtherAccounts(string username);

        [OperationContract]
        Account GetAccountByNo(string accountNo);

        [OperationContract]
        Account GetAccountById(int id);

        [OperationContract]
        string AddUserAccount(Account a);

        [OperationContract]
        bool UpdateAccount(Account a);

        [OperationContract]
        bool DeleteAccount(string accountNo);

        [OperationContract]
        IEnumerable<Account> GetAccounts();
       
        [OperationContract]
        IEnumerable<Account> GetAccounts1();

        [OperationContract]
        IEnumerable<Account> GetUserAccounts(string username);
    }
}
