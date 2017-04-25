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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TransactionTypesService" in code, svc and config file together.
    public class TransactionTypesService : ITransactionTypesService
    {
        public IEnumerable<TransactionType> GetTransactionTypes()
        {
            return new TransactionTypesRepository().GetTransactionTypes();
        }

        public TransactionType GetTransactionTypeId(string word)
        {
            return new TransactionTypesRepository().GetTransactionTypeId(word);
        }

        public TransactionType GetTransactionTypeById(int transactionTypeId)
        {
            return new TransactionTypesRepository().GetTransactionTypeById(transactionTypeId);
        }
    }
}
