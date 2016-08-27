using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ADV
{
    public class Device
    {
        #region private properties
        dtEntities1 db = new dtEntities1();
        #endregion
        #region private properties
        public string MessageCode { get; set; }
        #endregion
        #region public methods
        public IEnumerable<Data.DeviceType> GetList(decimal id)
        {
            return db.DeviceTypes.Where(m => (id == 0 || m.Id == id));
        }
        #endregion
    }
}
