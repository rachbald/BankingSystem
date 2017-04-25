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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IFixedRatesService" in both code and config file together.
    [ServiceContract]
    public interface IFixedRatesService
    {
        [OperationContract]
        IEnumerable<FixedRate> GetFixedRates();

        [OperationContract]
        FixedRate GetRateById(int id);
        
    }
}
