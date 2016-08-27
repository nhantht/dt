using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ADV
{
    public class Campaign
    {
        #region private properties
        dtEntities1 db = new dtEntities1();
        #endregion
        #region private properties
        public string MessageCode { get; set; }
        #endregion
        #region public methods
        public bool Create(Data.Campaign campaign)
        {
            try
            {
                //Create the nes id
                Data.Campaign last = db.Campaigns.OrderByDescending(m => m.CampaignId).FirstOrDefault();
                decimal id = 0;
                if (last != null)
                {
                    id = last.CampaignId;
                }
                id++;
                //Save the campaign
                campaign.CampaignId = id;
                campaign.CreatedDate = DateTime.Now;
                db.Campaigns.Add(campaign);
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
        public IEnumerable<Data.Campaign> GetList(decimal campaignId, string campaignName, string companyId)
        {
            return from u in db.Campaigns
                   where (u.CompanyId == companyId)
                   && (campaignId == 0 || campaignId == u.CampaignId)
                   && (string.IsNullOrEmpty(campaignName) || campaignName == u.Name)
                   orderby u.Name
                   select u;
        }
        public bool Update(Data.Campaign inputCampaign)
        {
            try
            {
                Data.Campaign campaign = db.Campaigns.Where(s => s.CampaignId == inputCampaign.CampaignId).FirstOrDefault();
                if (campaign != null)
                {
                    campaign.Name = inputCampaign.Name;
                    campaign.Status = inputCampaign.Status;
                    campaign.Budget = inputCampaign.Budget;

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
                Data.Campaign campaign = db.Campaigns.Where(s => s.CampaignId == id).FirstOrDefault();
                if (campaign != null)
                {
                    db.Campaigns.Remove(campaign);
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
