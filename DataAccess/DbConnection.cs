using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace DataAccess
{
    public class DbConnection
    {
        public DSAplcEntities Entity { get; set; }

        public DbConnection()
        {
            Entity = new DSAplcEntities();
        }

    }
}
