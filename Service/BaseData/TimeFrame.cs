using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.BaseData
{
    public class TimeFrame
    {
        #region private properties
        dtEntities1 db = new dtEntities1();
        string MessageCode = string.Empty;
        #endregion
        public List<Data.TimeFrame> GetList()
        {
            return db.TimeFrames.ToList();
        }
        public Data.TimeFrame GetById(decimal id)
        {
            return db.TimeFrames.Where(x => x.Id == id).FirstOrDefault();
        }
        public Data.TimeFrame Create(Data.TimeFrame input)
        {
            try
            {
                //Create the nes id
                Data.TimeFrame last = db.TimeFrames.OrderByDescending(m => m.Id).FirstOrDefault();
                decimal id = 0;
                if (last != null)
                {
                    id = last.Id;
                }
                id++;
                //Save the TimeFrame
                input.Id = id;
                db.TimeFrames.Add(input);
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
        public Data.TimeFrame Update(Data.TimeFrame input)
        {
            try
            {
                Data.TimeFrame TimeFrame = db.TimeFrames.Where(s => s.Id == input.Id).FirstOrDefault();
                if (TimeFrame != null)
                {
                    TimeFrame.Name = input.Name;
                    TimeFrame.FromHour = input.FromHour;
                    TimeFrame.ToHour = input.ToHour;

                    db.SaveChanges();
                }
                return TimeFrame;
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
                Data.TimeFrame TimeFrame = db.TimeFrames.Where(s => s.Id == id).FirstOrDefault();
                if (TimeFrame != null)
                {
                    db.TimeFrames.Remove(TimeFrame);
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
