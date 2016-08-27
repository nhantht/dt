using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.User
{
    public class Register
    {
        #region private properties
        dtEntities1 db = new dtEntities1();
        #endregion
        #region private properties
        public string Error { get; set; }
        #endregion
        public bool Active(string activeCode)
        {
            try
            {
                Data.User user = db.Users.Where(s => s.ActiveCode == activeCode).FirstOrDefault();
                if (user != null)
                {
                    user.IsActive = true;
                    user.StatusId = 2;
                    db.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public decimal CheckAndAddRegion(string city, string countryCode, string country)
        {
            try
            {
                decimal cityId = 0;
                decimal countryId = 0;

                if (db.Countries.Where(s => s.Code == countryCode).Count() == 0)
                {
                    //Add country
                    Country countryObj = db.Countries.OrderByDescending(s => s.CountryId).FirstOrDefault();
                    if (countryObj != null)
                    {
                        countryId = countryObj.CountryId;
                    }
                    countryId++;
                    db.Countries.Add(new Country
                    {
                        CountryId = countryId,
                        Code = countryCode,
                        Name = country
                    });
                    //Add city
                    cityId = AddCity(city, countryId);
                }
                else
                {
                    //Add city
                    cityId = AddCity(city, db.Countries.Where(s => s.Code == countryCode).FirstOrDefault().CountryId);
                }

                db.SaveChanges();

                return cityId;
            }
            catch (Exception error)
            {
                Error = "DuplicatedPhone";
                return 0;
            }
        }
        decimal AddCity(string city, decimal countryId)
        {
            //Add city
            decimal cityId = 0;

            if (db.Cities.Where(s => s.Name == city).Count() == 0)
            {
                City cityObj = db.Cities.OrderByDescending(s => s.CityId).FirstOrDefault();
                if (cityObj != null)
                {
                    cityId = cityObj.CityId;
                }
                cityId++;
                db.Cities.Add(new City
                {
                    CityId = cityId,
                    Name = city,
                    CountryId = countryId
                });
            }
            else
            {
                cityId = db.Cities.Where(s => s.Name == city).First().CityId;
            }

            return cityId;
        }
        public string GetTelephoneCode(string countryCode)
        {
            Data.TelephoneCode telephone = db.TelephoneCodes.Where(s => s.CountryCode == countryCode).FirstOrDefault();
            if (telephone != null)
            {
                return telephone.TelephoneCode1;
            }
            else
            {
                return string.Empty;
            }
        }

        public bool Create(Data.User user)
        {
            try
            {
                if (db.Users.Where(s => s.Phone == user.Phone).FirstOrDefault() != null)
                {
                    Error = "DuplicatedPhone";
                    return false;
                }
                if (db.Users.Where(s => s.Email == user.Email).FirstOrDefault() != null)
                {
                    Error = "DuplicatedEmail";
                    return false;
                }
                db.Users.Add(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception error)
            {
                Error = "PerformmingError";
                return false;
            }
        }
        #region publich methods
        #endregion
    }
}
