using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.User
{
    public class Login
    {
        #region private properties
        dtEntities1 db = new dtEntities1();
        #endregion
        public Data.User GetUserByPhone(string phone)
        {
            return db.Users.Where(s => s.Phone == phone).FirstOrDefault();
        }
    }
}
