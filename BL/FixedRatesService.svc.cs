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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "FixedRatesService" in code, svc and config file together.
    public class FixedRatesService : IFixedRatesService
    {
        public IEnumerable<FixedRate> GetFixedRates()
        {
            return new FixedRatesRepository().GetFixedRates();
        }

        public FixedRate GetRateById(int id)
        {
            return new FixedRatesRepository().GetRateById(id);
        }
    }
}
