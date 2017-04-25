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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CurrencesService" in code, svc and config file together.
    public class CurrencesService : ICurrencesService
    {
        public Currency GetCurrencyByCode(string code)
        {
            return new CurrenciesRepository().GetCurrencyByCode(code);
        }

        public IEnumerable<Currency> GetCurrencies()
        {
            return new CurrenciesRepository().GetCurrencies();
        }
    }
}
