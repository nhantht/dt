using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.BaseData
{
    public class Device
    {
        #region private properties
        dtEntities1 db = new dtEntities1();
        string MessageCode = string.Empty;
        #endregion
        public List<Data.Device> GetList()
        {
            return db.Devices.ToList();
        }
        public Data.Device GetByDeviceId(decimal id)
        {
            return db.Devices.Where(x => x.DeviceId == id).FirstOrDefault();
        }
        public Data.Device Create(Data.Device input)
        {
            try
            {
                //Create the nes id
                Data.Device last = db.Devices.OrderByDescending(m => m.DeviceId).FirstOrDefault();
                decimal id = 0;
                if (last != null)
                {
                    id = last.DeviceId;
                }
                id++;
                //Save the Device
                input.DeviceId = id;
                db.Devices.Add(input);
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
        public Data.Device Update(Data.Device input)
        {
            try
            {
                Data.Device Device = db.Devices.Where(s => s.DeviceId == input.DeviceId).FirstOrDefault();
                if (Device != null)
                {
                    Device.Name = input.Name;

                    db.SaveChanges();
                }
                return Device;
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
                Data.Device Device = db.Devices.Where(s => s.DeviceId == id).FirstOrDefault();
                if (Device != null)
                {
                    db.Devices.Remove(Device);
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
