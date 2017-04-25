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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "RolesService" in code, svc and config file together.
    public class RolesService : IRolesService
    {
        public Role GetRoleById(int id)
        {
            return new RolesRepository().GetRoleById( id);
        }

        public Role GetRoleId(string role)
        {
            return new RolesRepository().GetRoleId(role);
        }
    }
}
