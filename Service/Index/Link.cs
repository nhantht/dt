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
                Data.Link found = db.Links.OrderByDescending(m => m.Id).FirstOrDefault();
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
