using Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Service.Index
{
    public class Link
    {
        #region private properties
        dtEntities1 db = new dtEntities1();
        string MessageCode = string.Empty;
        #endregion
        #region Public methods
        public List<Data.Link> GetList(decimal? id)
        {
            return (from u in db.Links
                    where (id == null || (id != null && u.Id == id))
                    orderby u.CreatedDate descending
                    select u
            ).ToList();
        }
        public Data.Link GetByURL(string URL)
        {
            return (from u in db.Links
                    where (u.URL.Trim().ToLower() == URL.Trim().ToLower())
                    select u).FirstOrDefault();
        }
        public IEnumerable<Data.Link> GetByKeyword(string keyword, string priceOrder, string fromPrice, string toPrice, string currency)
        {
            double price1 = 0;
            double price2 = 0;
            
            if (!string.IsNullOrEmpty(fromPrice))
                price1 = double.Parse(fromPrice);
            if (!string.IsNullOrEmpty(toPrice))
                price2 = double.Parse(toPrice);

            IEnumerable<Data.Link> result = from u in db.Links
                   where (u.URL.Trim().ToLower().IndexOf(keyword.Trim().ToLower()) >= 0
                           || u.Title.Trim().ToLower().IndexOf(keyword.Trim().ToLower()) >= 0
                           || u.ShortDescription.Trim().ToLower().IndexOf(keyword.Trim().ToLower()) >= 0)
                           && (string.IsNullOrEmpty(fromPrice)
                               || u.Price >= price1)
                           && (string.IsNullOrEmpty(toPrice)
                               || u.Price <= price2)
                   select u;

            if (int.Parse(priceOrder) == 2)
            {
                result = result.OrderBy(x => x.Price);
            }
            else
            {
                if (int.Parse(priceOrder) == 3)
                {
                    result = result.OrderByDescending(x => x.Price);
                }
            }

            return result;
        }
        public bool CheckUnique(Data.Link link)
        {
            //Create the nes id
            Data.Link found = db.Links.Where(m => m.Id != link.Id && m.URL == link.URL).FirstOrDefault();
            return found != null;
        }
        public bool Delete(decimal id)
        {
            try
            {
                Data.Link link = db.Links.Where(s => s.Id == id).FirstOrDefault();
                if (link != null)
                {
                    db.Links.Remove(link);
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
        public bool Create(Data.Link link, bool UnanalysedPicture)
        {
            try
            {
                //Create the nes id
                Data.Link last = db.Links.OrderByDescending(m => m.Id).FirstOrDefault();
                decimal id = 0;
                if (last != null)
                {
                    id = last.Id;
                }
                id++;
                //Save the link
                link.Id = id;
                //Move picture
                if (!UnanalysedPicture && link.Picture.Length > 0)
                {
                    string[] parts = link.Picture.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                    if (string.IsNullOrEmpty(parts[parts.Length - 1]) == false)
                    {
                        link.Picture = link.UserId.TrimStart('+') + "_" + parts[parts.Length - 1];
                        string sourceFile = Path.Combine(Lib.Common.Variables.TempLinkImageFolderPath, link.Picture);
                        string destFile = Path.Combine(Lib.Common.Variables.LinkImageFolderPath, link.Picture);
                        link.Picture = Lib.Common.Application.ApplicationPath() + "/" + Lib.Common.Variables.LinkImageFolder + "/" + link.Picture;
                        File.Copy(sourceFile, destFile, true);
                    }
                }
                db.Links.Add(link);

                db.SaveChanges();

                //Clean
                last = null;
                return true;
            }
            catch (Exception error)
            {
                MessageCode = "PerformmingError";
                return false;
            }
        }
        public bool Update(Data.Link link, bool UnanalysedPicture)
        {
            try
            {
                Data.Link found = null;
                if (link.Id > 0)
                {
                    found = db.Links.OrderByDescending(m => m.Id == link.Id).FirstOrDefault();
                }
                else
                {
                    found = db.Links.OrderByDescending(m => m.URL.ToLower() == link.URL.Trim().ToLower()).FirstOrDefault();
                }

                if (found != null)
                {
                    //Save the link
                    found.URL = link.URL;
                    found.ShortDescription = link.ShortDescription;
                    found.Title = link.Title;
                    found.Price = link.Price;
                    //Move picture
                    if (!UnanalysedPicture && link.Picture.Length > 0)
                    {
                        string[] parts = link.Picture.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                        if (string.IsNullOrEmpty(parts[parts.Length - 1]) == false)
                        {
                            link.Picture = link.UserId.TrimStart('+') + "_" + parts[parts.Length - 1];
                            string sourceFile = Path.Combine(Lib.Common.Variables.TempLinkImageFolderPath, link.Picture);
                            string destFile = Path.Combine(Lib.Common.Variables.LinkImageFolderPath, link.Picture);
                            found.Picture = Lib.Common.Application.ApplicationPath() + "/" + Lib.Common.Variables.LinkImageFolder + "/" + link.Picture;
                            File.Copy(sourceFile, destFile, true);
                        }
                    }
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
