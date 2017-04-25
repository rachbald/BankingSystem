using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace DataAccess
{
    public class LogsRepository : DbConnection
    {
        public LogsRepository()
            : base ()
        {}

        public IEnumerable<Log> GetLogs()
        {
            return Entity.Logs.AsEnumerable();
        }

        public String GetLogsString()
        {
            return Entity.Logs.AsEnumerable().ToString();
        }

        public IEnumerable<Log> SortDescending()
        {
            return Entity.Logs.AsEnumerable().OrderByDescending(x => x.Date);
        }

        public IEnumerable<Log> SortAscending()
        {
            return Entity.Logs.AsEnumerable().OrderBy(x => x.Date);
        }

        
        public IEnumerable<Log> Filter(String startd, String endd, string clientIdstr, string accountIdstr)
        {
            IEnumerable<Log> logs = GetLogs();

            if (startd != "" && endd != "")
            {
                System.DateTime start = System.DateTime.Parse(startd);
                System.DateTime end = System.DateTime.Parse(endd);

                logs = logs.Intersect(Entity.Logs.Where(l => l.Date < start && l.Date > end));

            }

            if (clientIdstr != "")
            {
                int clientId = int.Parse(clientIdstr);
                logs = logs.Intersect(Entity.Logs.Where(x => x.Client == clientId));
            }

           /* if (accountIdstr != "")
            {
                int accountId = int.Parse(accountIdstr);
                logs = logs.Intersect(Entity.Logs.Where(x => x.a == clientId));
            }*/

            return logs;
            //int clientId = int.Parse(clientIdstr);
            //int accountId = int.Parse(accountIdstr);
            //if (!startd.Equals("") && !endd.Equals(""))
            //{
            //    System.DateTime start = System.DateTime.Parse(startd);
            //    System.DateTime end = System.DateTime.Parse(endd);

            //    if (clientId != 0)
            //    {
            //        if (accountId != 0)
            //        {
            //            //test += "accountId";
            //            return Entity.Logs.Where(l => (l.Date < start && l.Date > end) && l.Client.Equals(clientId) && l.Account.Equals(accountId));
                        
            //        }
            //        else
            //        {
            //            return Entity.Logs.Where(l => (l.Date < start && l.Date > end) && l.Client.Equals(clientId));
            //        }
            //    }
            //    else
            //    {
            //        if (accountId != 0)
            //        {
            //            return Entity.Logs.Where(l => (l.Date < start && l.Date > end) && l.Account.Equals(accountId));
            //        }
            //        else
            //        {
            //            return Entity.Logs.Where(l => (l.Date < start && l.Date > end));
            //        }
            //    }
            //}
            //else
            //{
            //    if (clientId != 0)
            //    {
            //        //test += "clientId";
            //        if (accountId != 0)
            //        {
            //            return Entity.Logs.Where(l => l.Client.Equals(clientId) && l.Account.Equals(accountId));
            //        }
            //        else
            //        {
            //            return Entity.Logs.Where(l => l.Client.Equals(clientId));
            //        }
            //    }
            //    else
            //    {
            //        if (accountId != 0)
            //        {
            //            return Entity.Logs.Where(l => l.Account.Equals(accountId));
            //        }
            //    }
            //}
            //return Entity.Logs.Where(l => l.Account.Equals(accountId));
        }

        public String FilterTest(String startd, String endd,  int clientId, int accountId)
        {
            string test = "";

            if (!startd.Equals("") && !endd.Equals(""))
            {
                System.DateTime  start = System.DateTime.Parse(startd);
                System.DateTime end = System.DateTime.Parse(endd);

                //test += "date";
                if (clientId != 0)
                {
                    //test += "clientId";
                    if (accountId != 0)
                    {
                        //test += "accountId";
                        //return Entity.Logs.Where(l => (l.Date < start && l.Date > end) && l.Client.Equals(clientId) && l.CurrentAccount.Equals(accountId));
                        test = "date + client + account";
                    }
                    else
                    {
                        test = "date + client";
                        //return Entity.Logs.Where(l => (l.Date < start && l.Date > end) && l.Client.Equals(clientId));
                    }
                }
                else
                {
                    if (accountId != 0)
                    {
                        test = "date + account";
                        //return Entity.Logs.Where(l => (l.Date < start && l.Date > end) && l.CurrentAccount.Equals(accountId));
                    }
                    else
                    {
                        test = "date";
                        //return Entity.Logs.Where(l => (l.Date < start && l.Date > end));
                    }
                }
            }
            else
            {
                if (clientId != 0)
                {
                    //test += "clientId";
                    if (accountId != 0)
                    {
                        test = "client + account";
                        //return Entity.Logs.Where(l => l.Client.Equals(clientId) && l.CurrentAccount.Equals(accountId));
                    }
                    else
                    {
                        test = "client";
                       // return Entity.Logs.Where(l => l.Client.Equals(clientId));
                    }
                }
                else
                {
                    if (accountId != 0)
                    {
                        test = "accountId";
                       // return Entity.Logs.Where(l => l.CurrentAccount.Equals(accountId));
                    }
                }
            }

            //return Entity.Logs.Where(l => ((l.Date < start && l.Date > end)));
            return test;
        }

        public bool AddLog(Log l)
        {
            try
            {
                Entity.AddToLogs(l);
                Entity.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
