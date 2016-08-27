using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Region
{
    public class CityObj
    {
        dtEntities1 db = new dtEntities1();
        public IEnumerable<City> GetAllCities()
        {
            return db.Cities.OrderBy(s => s.Name);
        }
    }
}
