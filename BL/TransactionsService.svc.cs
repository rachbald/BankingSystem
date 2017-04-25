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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TransactionsService" in code, svc and config file together.
    public class TransactionsService : ITransactionsService
    {
        public IEnumerable<Transaction> GetAccountTransactions(string accountNo)
        {
            return new TransactionRepository().GetAccountTransactions(accountNo);
        }

        public bool ProcessTransaction(Transaction t)
        {
            return new TransactionRepository().ProcessTransaction(t);
        }

        public Transaction GetTransactionById(string id)
        {
            return new TransactionRepository().GetTransactionById(id);
        }

        public IEnumerable<Transaction> FilterAccountTransactions(string accountNo, DateTime start, DateTime end)
        {
            return new TransactionRepository().FilterAccountTransactions(accountNo, start, end);
        }
    }
}
