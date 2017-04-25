using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace DataAccess
{
    public class CurrenciesRepository : DbConnection
    {
        public CurrenciesRepository()
            : base ()
        {}

        public IEnumerable<Currency> GetCurrencies()
        {
            return Entity.Currencies.AsEnumerable();
        }

        public Currency GetCurrencyByCode(string code)
        {
            return Entity.Currencies.SingleOrDefault(c => c.Code.Equals(code));
        }
    }
}
