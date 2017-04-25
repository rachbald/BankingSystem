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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "LogsService" in code, svc and config file together.
    public class LogsService : ILogsService
    {
        public bool AddLog(Log l)
        {
            return new LogsRepository().AddLog(l);
        }

        public IEnumerable<Log> GetLogs()
        {
            return new LogsRepository().GetLogs();
        }

        public String GetLogsString()
        {
            return new LogsRepository().GetLogsString();
        }

        public IEnumerable<Log> SortDescending()
        {
            return new LogsRepository().SortDescending();
        }

        public IEnumerable<Log> SortAscending()
        {
            return new LogsRepository().SortAscending();
        }

        public IEnumerable<Log> Filter(String startd, String endd, string clientId, string accountId)
        {
            return new LogsRepository().Filter(startd, endd, clientId, accountId);
        }

        public String FilterTest(String startd, String endd,  int clientId, int accountId)
        {
            return new LogsRepository().FilterTest(startd, endd, clientId, accountId);
        }
    }
}
