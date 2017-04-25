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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UsersService" in code, svc and config file together.
    public class UsersService : IUsersService
    {
        public bool IsAuthenticationValid(string username, string token)
        {
            return new UsersRepository().IsAuthenticationValid(username, token);
        }

        public bool Test(string username, string token)
        {
            return new UsersRepository().Test(username, token);
        }

        public User GetUserByUsername(string username)
        {
            return new UsersRepository().GetUserByUsername(username);
        }

        public IEnumerable<Common.Role> GetRolesForUsername(string username)
        {
            return new UsersRepository().GetRolesForUsername(username);
        }

        public void UpdateToken(string username)
        {
            new UsersRepository().UpdateToken(username);
        }

        public User GetUserById(int userId)
        { 
            return new UsersRepository().GetUserById(userId);
        }
    }
}
