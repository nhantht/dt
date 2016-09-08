using Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Service.ADV
{
    public enum Marching
    {
        [Description("Phase march")]
        _PhaseMarch = 1,
        _ExacMarch = 2,
        _None = 0
    }
    public class Keyword
    {
        #region private properties
        dtEntities1 db = new dtEntities1();
        #endregion
        #region private properties
        public string MessageCode { get; set; }
        #endregion
        #region public methods
        public bool Create(Data.Keyword model, List<Data.Device> devices)
        {
            try
            {
                if (db.Keywords.Where(m => m.Keyword1.ToLower().Trim() == model.Keyword1.ToLower().Trim()).FirstOrDefault() != null)
                {
                    return false;
                }
                //Add Keyword
                //Create the new id
                Data.Keyword last = db.Keywords.OrderByDescending(m => m.KeywordId).FirstOrDefault();
                decimal id = 0;
                if (last != null)
                {
                    id = last.KeywordId;
                }
                id++;
                //Save the Keyword
                Data.Keyword newKeyword = new Data.Keyword
                {
                    Keyword1 = model.Keyword1,
                    Budget = model.Budget,
                    Status = model.Status,
                    Marching = (short)model.Marching,
                    Bid = model.Bid,
                    URLId = model.URLId
                };

                newKeyword.KeywordId = id;

                db.Keywords.Add(newKeyword);
                if (devices != null)
                {
                    foreach (Data.Device device in devices)
                    {
                        db.NegativeMarchs.Add(new Data.NegativeMarch
                        {
                            DeviceId = device.DeviceId,
                            KeywordId = newKeyword.KeywordId
                        });
                    }
                }
                db.SaveChanges();
                //Clean
                last = null;
                return true;
            }
            catch (Exception error)
            {
                MessageCode = "DuplicatedKeyword";
                return false;
            }
        }
        public IEnumerable<Data.Keyword> GetList(decimal Id, decimal URLId, string Keyword, Marching Marching)
        {
            IEnumerable<Data.Keyword> Keywords = from u in db.Keywords
                                                 where 1 == 1
                                                 && (Marching == Marching._None || u.Marching == (short)Marching)
                                                 && (Id == 0 || Id == u.KeywordId)
                                                 && (string.IsNullOrEmpty(Keyword) || Keyword == u.Keyword1)
                                                 && (URLId == 0 || u.URLId == URLId)
                                                 orderby u.Keyword1
                                                 select u;

            return Keywords;
        }
        public IEnumerable<Data.Keyword> GetListByKeyword(string keyword)
        {
            IEnumerable<Data.Keyword> Keywords = from u in db.Keywords
                                                 where 1 == 1
                                                 && u.Keyword1.ToLower().Trim().IndexOf(keyword.Trim().ToLower()) >= 0
                                                 orderby u.Keyword1
                                                 select u;

            return Keywords;
        }
        public IEnumerable<Data.Device> GetDeviceList()
        {
            IEnumerable<Data.Device> devices = from u in db.Devices
                                               orderby u.Name
                                               select u;

            return devices;
        }
        public IEnumerable<Data.Device> GetNegativeMarchs(decimal keywordId)
        {
            IEnumerable<Data.Device> devices = from u in db.NegativeMarchs
                                               join m in db.Devices on u.DeviceId equals m.DeviceId
                                               where u.KeywordId == keywordId
                                               select m;

            return devices;
        }
        public bool Update(Data.Keyword inputKeyword, List<Data.Device> devices)
        {
            try
            {
                Data.Keyword Keyword = db.Keywords.Where(s => s.KeywordId == inputKeyword.KeywordId).FirstOrDefault();
                if (Keyword != null)
                {
                    Keyword.Keyword1 = inputKeyword.Keyword1;
                    Keyword.Status = inputKeyword.Status;
                    Keyword.Budget = inputKeyword.Budget;
                    Keyword.Bid = inputKeyword.Bid;
                    Keyword.Marching = inputKeyword.Marching;

                    //Delete negative marchs
                    foreach (Data.NegativeMarch device in Keyword.NegativeMarchs.ToList())
                    {
                        db.NegativeMarchs.Remove(device);
                    }
                    foreach (Data.Device device in devices)
                    {
                        db.NegativeMarchs.Add(new NegativeMarch { DeviceId = device.DeviceId, KeywordId = inputKeyword.KeywordId });
                    }
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
        public bool Dekete(decimal id)
        {
            try
            {
                Data.Keyword Keyword = db.Keywords.Where(s => s.KeywordId == id).FirstOrDefault();
                if (Keyword != null)
                {
                    db.Keywords.Remove(Keyword);
                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception error)
            {
                MessageCode = "InuseKeyword";
                return false;
            }
        }
        #endregion
    }
}
