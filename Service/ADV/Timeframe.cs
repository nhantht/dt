using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ADV
{
    public class Timeframe
    {
        #region private properties
        dtEntities1 db = new dtEntities1();
        #endregion
        #region private properties
        public string MessageCode { get; set; }
        #endregion
        #region public methods
        public IEnumerable<Data.TimeFrame> GetList(decimal Id)
        {
            IEnumerable<Data.TimeFrame> timeframes = from u in db.TimeFrames
                             where 1 == 1
                             && (Id == 0 || Id == u.Id)
                             orderby u.FromHour
                             select u;

            return timeframes;
        }
        #endregion
    }
}
