using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.BaseData
{
    public class DeviceType
    {
        #region private properties
        dtEntities1 db = new dtEntities1();
        string MessageCode = string.Empty;
        #endregion
        public List<Data.DeviceType> GetList()
        {
            return db.DeviceTypes.ToList();
        }
        public Data.DeviceType GetById(decimal id)
        {
            return db.DeviceTypes.Where(x => x.Id == id).FirstOrDefault();
        }
        public Data.DeviceType Create(Data.DeviceType input)
        {
            try
            {
                //Create the nes id
                Data.DeviceType last = db.DeviceTypes.OrderByDescending(m => m.Id).FirstOrDefault();
                decimal id = 0;
                if (last != null)
                {
                    id = last.Id;
                }
                id++;
                //Save the DeviceType
                input.Id = id;
                db.DeviceTypes.Add(input);
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
        public Data.DeviceType Update(Data.DeviceType input)
        {
            try
            {
                Data.DeviceType DeviceType = db.DeviceTypes.Where(s => s.Id == input.Id).FirstOrDefault();
                if (DeviceType != null)
                {
                    DeviceType.Name = input.Name;

                    db.SaveChanges();
                }
                return DeviceType;
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
                Data.DeviceType DeviceType = db.DeviceTypes.Where(s => s.Id == id).FirstOrDefault();
                if (DeviceType != null)
                {
                    db.DeviceTypes.Remove(DeviceType);
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
