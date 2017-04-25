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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITransactionsService" in both code and config file together.
    [ServiceContract]
    public interface ITransactionsService
    {
        [OperationContract]
        IEnumerable<Transaction> GetAccountTransactions(string accountNo);

        [OperationContract]
        bool ProcessTransaction(Transaction t);

        [OperationContract]
        Transaction GetTransactionById(string id);

        [OperationContract]
        IEnumerable<Transaction> FilterAccountTransactions(string accountNo, DateTime start, DateTime end);
    }
}
