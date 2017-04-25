using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace DataAccess
{
    public class RolesRepository : DbConnection
    {
        public RolesRepository()
            : base()
        { }

        public Role GetRoleById(int id)
        {
            return Entity.Roles.SingleOrDefault(r => r.Id.Equals(id));
        }

        public Role GetRoleId(string role)
        {
            return Entity.Roles.SingleOrDefault(r => r.Name.Equals(role));
        }
    }
}
