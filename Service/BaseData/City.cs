using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.BaseData
{
    public class City
    {
        #region private properties
        dtEntities1 db = new dtEntities1();
        string MessageCode = string.Empty;
        #endregion
        public List<Data.City> GetList()
        {
            return db.Cities.OrderBy(x=>x.Name).ToList();
        }
        public Data.City GetById(decimal id)
        {
            return db.Cities.Where(x => x.CityId == id).FirstOrDefault();
        }
        public Data.City Create(Data.City input)
        {
            try
            {
                //Create the nes id
                Data.City last = db.Cities.OrderByDescending(m => m.CityId).FirstOrDefault();
                decimal id = 0;
                if (last != null)
                {
                    id = last.CityId;
                }
                id++;
                //Save the City
                input.CityId = id;
                db.Cities.Add(input);
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
        public Data.City Update(Data.City input)
        {
            try
            {
                Data.City City = db.Cities.Where(s => s.CityId == input.CityId).FirstOrDefault();
                if (City != null)
                {
                    City.Name = input.Name;
                    City.CountryId = input.CountryId;

                    db.SaveChanges();
                }
                return City;
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
                Data.City City = db.Cities.Where(s => s.CityId == id).FirstOrDefault();
                if (City != null)
                {
                    db.Cities.Remove(City);
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
