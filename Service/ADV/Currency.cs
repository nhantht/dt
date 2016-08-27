using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ADV
{
    public class Currency
    {
        #region private properties
        dtEntities1 db = new dtEntities1();
        #endregion
        #region private properties
        public string MessageCode { get; set; }
        #endregion
        #region public methods
        public IEnumerable<Data.Currency> GetList()
        {
            return db.Currencies.OrderBy(m=>m.Name);
        }
        #endregion
    }
}
