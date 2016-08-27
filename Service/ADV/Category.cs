using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ADV
{
    public class Category
    {
        #region private properties
        dtEntities1 db = new dtEntities1();
        #endregion
        #region private properties
        public string MessageCode { get; set; }
        #endregion
        #region public methods
        public bool Create(Data.Category Category, decimal campaignId)
        {
            try
            {
                //Create the nes id
                Data.Category last = db.Categories.OrderByDescending(m => m.CategoryId).FirstOrDefault();
                decimal id = 0;
                if (last != null)
                {
                    id = last.CategoryId;
                }
                id++;
                //Save the Category
                Category.CategoryId = id;
                Category.CreatedDate = DateTime.Now;
                Category.Campaigns.Add(db.Campaigns.Where(m => m.CampaignId == campaignId).FirstOrDefault());

                db.Categories.Add(Category);
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
        public IEnumerable<Data.View_CategoryList> GetList(decimal categoryId, string categoryName, decimal campaignId)
        {
            IEnumerable<Data.View_CategoryList> categories = from u in db.View_CategoryList
                             where 1 == 1
                             && u.CampaignId == campaignId
                             && (categoryId == 0 || categoryId == u.CategoryId)
                             && (string.IsNullOrEmpty(categoryName) || categoryName == u.Name)
                             orderby u.Name
                             select u;

            return categories;
        }
        public bool Update(Data.Category inputCategory)
        {
            try
            {
                Data.Category Category = db.Categories.Where(s => s.CategoryId == inputCategory.CategoryId).FirstOrDefault();
                if (Category != null)
                {
                    Category.Name = inputCategory.Name;
                    Category.Status = inputCategory.Status;
                    Category.Budget = inputCategory.Budget;

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
                Data.Category Category = db.Categories.Where(s => s.CategoryId == id).FirstOrDefault();
                if (Category != null)
                {
                    db.Categories.Remove(Category);
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
        #endregion
    }
}
