using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Region
{
    public class CountryObj
    {
        dtEntities1 db = new dtEntities1();
        public IEnumerable<Country> GetAvailableLanguages(string patern)
        {
            return from s in db.Countries
                   where (patern.IndexOf(s.Code.Replace("-", "_")) >= 0)
                   orderby s.Name
                   select s;
        }
        public IEnumerable<Country> GetAllCountries(decimal id, string code)
        {
            return from s in db.Countries
                   where (id == 0 || s.CountryId == id)
                   && (string.IsNullOrEmpty(code) || s.Code == code.Replace("_", "-"))
                   orderby s.Name
                   select s;
        }
    }
}
