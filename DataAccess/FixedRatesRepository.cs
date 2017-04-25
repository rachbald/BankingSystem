using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace DataAccess
{
    public class FixedRatesRepository : DbConnection
    {
        public FixedRatesRepository()
            : base()
        {
        }

        public IEnumerable<FixedRate> GetFixedRates()
        {
            return Entity.FixedRates.AsEnumerable();
        }

        public FixedRate GetRateById(int id)
        {
            return Entity.FixedRates.SingleOrDefault(r => r.Id.Equals(id));
        }
    }
}
