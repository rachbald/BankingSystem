using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Common;

namespace BL
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IUsersService" in both code and config file together.
    [ServiceContract]
    public interface IUsersService
    {
        [OperationContract]
        bool IsAuthenticationValid(string username, string token);

        [OperationContract]
        bool Test(string username, string token);
        

        [OperationContract]
        User GetUserByUsername(string username);

        [OperationContract]
        IEnumerable<Role> GetRolesForUsername(string username);

        [OperationContract]
        void UpdateToken(string username);

        [OperationContract]
        User GetUserById(int userId);
    }
}
