using Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace Service.Index
{
    public class Schedule
    {
        #region private properties
        dtEntities1 db = new dtEntities1();
        string MessageCode = string.Empty;
        #endregion
        #region Public methods
        public List<Data.Schedule> GetList(decimal? id)
        {
            return (from u in db.Schedules
                    where (id == null || (id != null && u.Id == id))
                    orderby u.CreatedDate descending
                    select u
            ).ToList();
        }
        public string RunSchedule(decimal id, string userId)
        {
            Link linkObj = new Link();
            string message = string.Empty;
            List<Data.Schedule> list = GetList(id);
            if (list.Count == 0)
            {
                return "This schedule was deleted!";
            }

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(list[0].FilePath);
            }
            catch
            {
                return "URL is invalid!";
            }
            XmlNodeList root = doc.GetElementsByTagName("LINKS");
            if (root.Count == 0)
            {
                return "The structure is wrong";
            }
            Data.Link link = new Data.Link();
            int i = 2;
            foreach (XmlNode node in root[0].ChildNodes)
            {
                link = new Data.Link();
                link.CreatedDate = DateTime.Now;
                link.UserId = userId;
                //URL
                if (!string.IsNullOrEmpty(node.ChildNodes[0].InnerText))
                {
                    link.URL = node.ChildNodes[0].InnerText;
                    //Title
                    if (!string.IsNullOrEmpty(node.ChildNodes[1].InnerText))
                    {
                        link.Title = node.ChildNodes[2].InnerText;
                    }
                    else
                    {
                        link.Title = string.Empty;
                    }
                    //ShortDescription
                    if (!string.IsNullOrEmpty(node.ChildNodes[2].InnerText))
                    {
                        link.ShortDescription = node.ChildNodes[2].InnerText;
                    }
                    else
                    {
                        link.ShortDescription = string.Empty;
                    }
                    //Picture
                    if (!string.IsNullOrEmpty(node.ChildNodes[3].InnerText))
                    {
                        link.Picture = node.ChildNodes[3].InnerText;
                    }
                    //URL
                    if (!string.IsNullOrEmpty(node.ChildNodes[4].InnerText))
                    {
                        link.Price = float.Parse(node.ChildNodes[4].InnerText);
                    }
                }
                Data.Link found = linkObj.GetByURL(link.URL);
                if (linkObj.GetByURL(link.URL) != null)
                {
                    //Update
                    link.Id = found.Id;
                    if (linkObj.Update(link, true) == false)
                    {
                        AddErrorMessage(ref message, i);
                    }
                }
                else
                {
                    if (linkObj.Create(link, true) == false)
                    {
                        AddErrorMessage(ref message, i);
                    }
                }
                i++;
            }
            if (message.Length > 0)
            {
                message = "The following rows are invalid: " + message + ". \nPlease check again.";
            }
            //Clean
            doc = null;
            return message;
        }
        void AddErrorMessage(ref string message, int rowIndex)
        {
            if (message.Length > 0)
            {
                message += ", ";
            }
            message += rowIndex.ToString();
        }
        public bool CheckUnique(Data.Schedule schedule)
        {
            //Create the nes id
            Data.Schedule found = db.Schedules.Where(m => m.Id != schedule.Id && m.FilePath == schedule.FilePath).FirstOrDefault();
            return found != null;
        }
        public bool Delete(decimal id)
        {
            try
            {
                Data.Schedule schedule = db.Schedules.Where(s => s.Id == id).FirstOrDefault();
                if (schedule != null)
                {
                    db.Schedules.Remove(schedule);
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
        public bool Create(Data.Schedule schedule)
        {
            try
            {
                db.Schedules.Add(schedule);

                db.SaveChanges();

                //Clean
                return true;
            }
            catch (Exception error)
            {
                MessageCode = "PerformmingError";
                return false;
            }
        }
        public bool Update(Data.Schedule schedule)
        {
            try
            {
                Data.Schedule found = db.Schedules.OrderByDescending(m => m.Id == schedule.Id).FirstOrDefault();
                if (found != null)
                {
                    //Save the schedule
                    found.FilePath = schedule.FilePath;
                    found.Hour = schedule.Hour;
                    found.Minute = schedule.Minute;
                    found.Active = schedule.Active;

                    db.SaveChanges();
                }
                //Clean
                found = null;
                return true;
            }
            catch (Exception error)
            {
                MessageCode = "PerformmingError";
                return false;
            }
        }
        #endregion
    }
}
