using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Common;

namespace BL
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ILogsService" in both code and config file together.
    [ServiceContract]
    public interface ILogsService
    {
        [OperationContract]
        IEnumerable<Log> GetLogs();

        [OperationContract]
        String GetLogsString();

        [OperationContract]
        bool AddLog(Log l);

        [OperationContract]
        IEnumerable<Log> SortDescending();

        [OperationContract]
        IEnumerable<Log> SortAscending();

        [OperationContract]
        IEnumerable<Log> Filter(String startd, String endd,  string clientId, string accountId);

        [OperationContract]
        String FilterTest(String startd, String endd, int clientId, int accountId);
    }
}
