using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.BaseData
{
    public class Currency
    {
        #region private properties
        dtEntities1 db = new dtEntities1();
        string MessageCode = string.Empty;
        #endregion
        public List<Data.Currency> GetList()
        {
            return db.Currencies.ToList();
        }
        public Data.Currency GetById(decimal id)
        {
            return db.Currencies.Where(x => x.Id == id).FirstOrDefault();
        }
        public Data.Currency Create(Data.Currency input)
        {
            try
            {
                //Create the nes id
                Data.Currency last = db.Currencies.OrderByDescending(m => m.Id).FirstOrDefault();
                decimal id = 0;
                if (last != null)
                {
                    id = last.Id;
                }
                id++;
                //Save the currency
                input.Id = id;
                db.Currencies.Add(input);
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
        public Data.Currency Update(Data.Currency input)
        {
            try
            {
                Data.Currency currency = db.Currencies.Where(s => s.Id == input.Id).FirstOrDefault();
                if (currency != null)
                {
                    currency.Code = input.Code;
                    currency.Name = input.Name;

                    db.SaveChanges();
                }
                return currency;
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
                Data.Currency currency = db.Currencies.Where(s => s.Id == id).FirstOrDefault();
                if (currency != null)
                {
                    db.Currencies.Remove(currency);
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
