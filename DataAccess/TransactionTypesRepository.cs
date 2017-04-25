using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace DataAccess
{
    public class TransactionTypesRepository : DbConnection
    {
        public TransactionTypesRepository()
            : base()
        { }

        public IEnumerable<TransactionType> GetTransactionTypes()
        {
            return Entity.TransactionTypes.AsEnumerable();
        }

        public TransactionType GetTransactionTypeId(string word)
        {
            return Entity.TransactionTypes.SingleOrDefault(t => t.Transaction.Contains(word));
        }

        public TransactionType GetTransactionTypeById(int transactionTypeId)
        {
            return Entity.TransactionTypes.SingleOrDefault(t => t.Id.Equals(transactionTypeId));
        }
    }
}
