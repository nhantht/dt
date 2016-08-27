using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.BaseData
{
    public class Country
    {
        #region private properties
        dtEntities1 db = new dtEntities1();
        string MessageCode = string.Empty;
        #endregion
        public List<Data.Country> GetList()
        {
            return db.Countries.OrderBy(x=>x.Name).ToList();
        }
        public Data.Country GetByCountryId(decimal id)
        {
            return db.Countries.Where(x => x.CountryId == id).FirstOrDefault();
        }
        public Data.Country Create(Data.Country input)
        {
            try
            {
                //Create the nes id
                Data.Country last = db.Countries.OrderByDescending(m => m.CountryId).FirstOrDefault();
                decimal id = 0;
                if (last != null)
                {
                    id = last.CountryId;
                }
                id++;
                //Save the Country
                input.CountryId = id;
                db.Countries.Add(input);
                db.SaveChanges();

                //Clean
                last = null;
                return input;
            }
            catch (Exception error)
            {
                MessageCode = "PerformmingError";
                return null;
            }
        }
        public Data.Country Update(Data.Country input)
        {
            try
            {
                Data.Country Country = db.Countries.Where(s => s.CountryId == input.CountryId).FirstOrDefault();
                if (Country != null)
                {
                    Country.Name = input.Name;
                    Country.Code = input.Code;
                    Country.TelephoneCode = input.TelephoneCode;

                    db.SaveChanges();
                }
                return Country;
            }
            catch (Exception error)
            {
                MessageCode = "PerformmingError";
                return null;
            }
        }
        public bool Delete(decimal id)
        {
            try
            {
                Data.Country Country = db.Countries.Where(s => s.CountryId == id).FirstOrDefault();
                if (Country != null)
                {
                    db.Countries.Remove(Country);
                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception error)
            {
                MessageCode = "PerformmingError";
                return false;
            }
        }
    }
}
