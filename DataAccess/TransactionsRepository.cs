using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace DataAccess
{
    public class TransactionsRepository : DbConnection
    {
        public TransactionsRepository()
            : base()
        {

        }

        public IEnumerable<Transaction> GetAccountTransactions(string accountNo)
        {
            return Entity.Transactions.Where(a => a.CurrentAccount.Equals(accountNo));
        }

        public Transaction GetTransactionById(string id)
        {
            return Entity.Transactions.SingleOrDefault(a => a.Id.Equals(id));
        }

        public bool ProcessTransaction(Transaction t)
        {
            try
            {
                Entity.AddToTransactions(t);
                Entity.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public IEnumerable<Transaction> FilterAccountTransactions(string accountNo, DateTime start, DateTime end)
        {
            return Entity.Transactions.Where(a => (a.CurrentAccount.Equals(accountNo)) && (a.Date > start && a.Date < end));
        }
    }
}