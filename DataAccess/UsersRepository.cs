using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace DataAccess
{
    public class UsersRepository : DbConnection
    {
        public UsersRepository()
            : base()
        {
        }

        public bool IsAuthenticationValid(string username, string token)
        {
            /*
            User user = Entity.Users.SingleOrDefault(u => u.Username == username);
            

            string token2 = user.Token.Replace('_', '+');
            token2 = Utilities.Encryption.DecryptTripleDES(token2);

            if (token2 == token)
            {
                return true;
            }
            */
            return false;
        }

        public bool Test(string username, string token)
        {
            Utilities.Encryption.CryptoTypes crypto = new Utilities.Encryption.CryptoTypes();
            Utilities.Encryption enc = new Utilities.Encryption();

            //return Entity.Users.SingleOrDefault(u => u.Username == username && u.Pin == pin) != null ;
            User user = Entity.Users.SingleOrDefault(u => u.Username == username);
            string savedToken = user.Token;
            int algorithm = user.EncryptionAlgorithm;

            switch (algorithm)
            {
                case 0:
                    crypto = Utilities.Encryption.CryptoTypes.encTypeTripleDES;
                    break;
                case 1:
                    crypto = Utilities.Encryption.CryptoTypes.encTypeRC2;
                    break;
                case 2:
                    crypto = Utilities.Encryption.CryptoTypes.encTypeRijndael;
                    break;
                default:
                    return false;
                    break;
            }


            if (token == savedToken)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public User GetUserByUsername(string username)
        {
            return Entity.Users.SingleOrDefault(x => x.Username.Equals(username));
        }

        public User GetUserById(int userId)
        {
            return Entity.Users.SingleOrDefault(x => x.Id.Equals(userId));
        }

        public IEnumerable<Role> GetRolesForUsername(string username)
        {
            return GetUserByUsername(username).Roles;
        }

        public void UpdateToken(string username)
        {
            User user = GetUserByUsername(username);

            Entity.Users.Attach(GetUserByUsername(username));

            user.Token = "0";
            Entity.Users.ApplyCurrentValues(user);
            Entity.SaveChanges();
        }
    }
}