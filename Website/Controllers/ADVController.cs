using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.IO;
using System.Net;
using HtmlAgilityPack;
using System.Text;
using Website.Models;

namespace Website.Controllers
{
    public class ADVController : Controller
    {
        Service.ADV.Campaign campaign = new Service.ADV.Campaign();
        Service.ADV.Category category = new Service.ADV.Category();
        Service.ADV.URL URL = new Service.ADV.URL();
        Service.Index.Link linkObj = new Service.Index.Link();
        Service.ADV.Keyword keyword = new Service.ADV.Keyword();
        [Authorize]
        public ActionResult Index()
        {
            List<Service.ADV.DeviceTypeInput> devices = new List<Service.ADV.DeviceTypeInput>();
            devices.Add(new Service.ADV.DeviceTypeInput
            {
                DeviceTypeId = 1,
                Title = "A",
                Picture = null
            });

            List<Service.ADV.PriceInput> prices = new List<Service.ADV.PriceInput>();
            prices.Add(new Service.ADV.PriceInput
            {
                CurrencyId = 1,
                Price = 12
            });

            URL.ImageFolderPath = Server.MapPath("~/Uploads/");
            URL.TempImageFolderPath = Server.MapPath("~/TempUploads/");
            URL.LoginedUser = User.Identity.Name;
            URL.TestCreate(new Service.ADV.InputURLViewModels
            {
                Id = 1,
                URL = "a",
                Budget = 10,
                AutoPictureUrl = "http://shac.vn/wp-content/uploads/2015/01/ngoi-nha-co-mat-tien-6m50-rong-sh-nod-0100.jpg",
                Status = true,
                CategoryId = 1,
                DisplayTimes = 1000,
                DeviceTypeInputs = devices,
                PriceInputs = prices
            });
            return View();
        }
        [Authorize]
        public ActionResult ReportInvalidURL(string reason, string URL)
        {
            Service.User.Manage manageObj = new Service.User.Manage();
            bool result = manageObj.ReportInvalidURL(URL, reason, User.Identity.Name);
            return Json(result);
        }
        public ActionResult Search(string keyword, string priceOrder, string fromPrice, string toPrice, string currency, int page = 1)
        {
            int pageSize = 10;
            SearchResult result = new SearchResult();
            Service.ADV.Keyword keywordObj = new Service.ADV.Keyword();
            List<IndexDetailModel> tempLinks = new List<IndexDetailModel>();
            result.Links = tempLinks.ToPagedList(page, pageSize);
            result.RelativeKeywords = new List<KeywordListViewModels>();
            List<Service.ADV.SearchURL> tempURLs = new List<Service.ADV.SearchURL>();
            result.URLs = tempURLs.ToPagedList(page, pageSize);

            if (priceOrder == null)
            {
                priceOrder = "1";
            }
            if (currency == null)
            {
                currency = "VND";
            }

            result.Keyword = keyword;
            result.PriceOrder = int.Parse(priceOrder);
            result.FromPrice = fromPrice;
            result.ToPrice = toPrice;
            result.Currency = currency;

            if (string.IsNullOrEmpty(keyword) == false)
            {
                string[] keywords = keyword.Split(new string[]{" "}, StringSplitOptions.RemoveEmptyEntries);
                foreach (string key in keywords.ToList())
                {
                    result.RelativeKeywords.AddRange(from u in keywordObj.GetListByKeyword(key)
                                                     select new Models.KeywordListViewModels
                                                               {
                                                                   Keyword = u.Keyword1
                                                               });
                    tempLinks.AddRange(from u in linkObj.GetByKeyword(key, priceOrder, fromPrice, toPrice, currency)
                                          select new Models.IndexDetailModel
                                                    {
                                                        Id = u.Id,
                                                        Title = u.Title,
                                                        ShortDescription = u.ShortDescription,
                                                        Picture = u.Picture,
                                                        CreatedDate = u.CreatedDate,
                                                        URL = u.URL,
                                                        Price = u.Price,
                                                        Rating = u.Rating,
                                                        Reviews = u.Reviews,
                                                    });

                    tempURLs.AddRange(URL.GetByKeyword(key, priceOrder, fromPrice, toPrice, currency, 1));
                }
                result.RelativeKeywords = result.RelativeKeywords.Distinct().ToList();
                result.Links = tempLinks.Distinct().ToPagedList(page, pageSize);
                result.URLs = tempURLs.Distinct().ToPagedList(1, 90000);
            }

            return View(result);
        }
        public ActionResult GetURLContent(string url)
        {
            string title = string.Empty;
            string description = string.Empty;
            string img = string.Empty;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                string result = string.Empty;
                try
                {
                    using (var stream = request.GetResponse().GetResponseStream())

                    using (var reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        result = reader.ReadToEnd();
                    }
                }
                catch (Exception err)
                {
                    string str = err.Message;
                }
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(result);
                //Title
                title = GetMeta(string.Format("//meta[@property='{0}']", "og:title"), doc);

                if (string.IsNullOrEmpty(title))
                {
                    title = GetMeta(string.Format("//meta[@name='{0}']", "title"), doc);
                }

                description = GetMeta(string.Format("//meta[@property='{0}']", "og:description"), doc);

                if (string.IsNullOrEmpty(description))
                {
                    description = GetMeta(string.Format("//meta[@name='{0}']", "description"), doc);
                }

                img = GetMeta(string.Format("//meta[@property='{0}']", "og:image"), doc);

                if (string.IsNullOrEmpty(img))
                {
                    img = GetMeta("//img", doc, "src");
                    if (!img.StartsWith("http"))
                    {
                        img = "http://" + request.RequestUri.Authority + img;
                    }
                }
            }
            catch { }
            return Json(new { Title = title, Description = description, Image = img });
        }
        string GetMeta(string partem, HtmlDocument doc, string key = "content")
        {
            string result = string.Empty;
            HtmlNode ourNode = doc.DocumentNode.SelectSingleNode(partem);
            if (ourNode != null)
            {
                result = ourNode.GetAttributeValue(key, "");
            }
            else
            {
                result = string.Empty;
            }

            return result;
        }
        #region Campaign
        // GET: ADV
        public ActionResult CampaignList(int page = 1)
        {
            IEnumerable<Models.CampaignListViewModels> result = from u in campaign.GetList(0, string.Empty, User.Identity.Name)
                                                                select new Models.CampaignListViewModels
                                                                {
                                                                    Id = u.CampaignId,
                                                                    Name = u.Name,
                                                                    Budget = u.Budget,
                                                                    Status = u.Status,
                                                                    DisplayTimes = 60,
                                                                    CTR = 10,
                                                                    AverageCPD = 2,
                                                                    Cost = 90
                                                                };

            int pageSize = 8;

            return PartialView(result.ToPagedList(page, pageSize));
        }
        public ActionResult CampaignDetail(decimal id)
        {
            Models.EditCampaignViewModels result = (from u in campaign.GetList(id, string.Empty, User.Identity.Name)
                                                    select new Models.EditCampaignViewModels
                                                                {
                                                                    Id = u.CampaignId,
                                                                    Name = u.Name,
                                                                    Budget = u.Budget,
                                                                    Status = u.Status
                                                                }).FirstOrDefault();

            return PartialView(result);
        }
        public ActionResult DeleteCampaign(decimal id)
        {
            string message = string.Empty;
            if (!campaign.Dekete(id))
            {
                message = Website.App_LocalResources.Error.InuseCampaign;
            }

            return Json(new { result = message });
        }
        public ActionResult EditCampaign(Models.EditCampaignViewModels model)
        {
            model.Message = string.Empty;
            if (!campaign.Update(new Data.Campaign
            {
                CampaignId = model.Id,
                Name = model.Name,
                Budget = model.Budget,
                Status = model.Status
            }))
            {
                model.Message = Website.App_LocalResources.Error.PerformmingError;
            }

            return Json(new { result = model.Message });
        }
        public ActionResult CreateCampaign()
        {
            return PartialView(new Models.EditCampaignViewModels
            {
                Status = false
            });
        }
        [HttpPost]
        public ActionResult CreateCampaign(Models.EditCampaignViewModels model)
        {
            model.Message = string.Empty;
            if (campaign.Create(new Data.Campaign
            {
                Name = model.Name,
                Budget = model.Budget,
                Status = model.Status,
                CompanyId = User.Identity.Name
            }) == false)
            {
                model.Message = Website.App_LocalResources.Error.PerformmingError;
            }

            return Json(new { result = model.Message });
        }
        #endregion
        #region Category
        // GET: ADV
        public ActionResult CategoryList(decimal campaignId, int page = 1)
        {
            IEnumerable<Models.CategoryListViewModels> result = from u in category.GetList(0, string.Empty, campaignId)
                                                                select new Models.CategoryListViewModels
                                                                {
                                                                    Id = u.CategoryId,
                                                                    Name = u.Name,
                                                                    Budget = u.Budget,
                                                                    Status = u.Status,
                                                                    DisplayTimes = 60,
                                                                    CTR = 10,
                                                                    AverageCPD = 2,
                                                                    Cost = 90
                                                                };

            int pageSize = 8;

            return PartialView(result.ToPagedList(page, pageSize));
        }
        public ActionResult CategoryDetail(decimal id, decimal campaignId)
        {
            Models.EditCategoryViewModels result = (from u in category.GetList(id, string.Empty, campaignId)
                                                    select new Models.EditCategoryViewModels
                                                    {
                                                        Id = u.CategoryId,
                                                        Name = u.Name,
                                                        Budget = u.Budget,
                                                        Status = u.Status
                                                    }).FirstOrDefault();

            return PartialView(result);
        }
        public ActionResult DeleteCategory(decimal id)
        {
            string message = string.Empty;
            if (!category.Dekete(id))
            {
                message = Website.App_LocalResources.Error.PerformmingError;
            }

            return Json(new { result = message });
        }
        public ActionResult EditCategory(Models.EditCategoryViewModels model)
        {
            model.Message = string.Empty;
            if (!category.Update(new Data.Category
            {
                CategoryId = model.Id,
                Name = model.Name,
                Budget = model.Budget,
                Status = model.Status
            }))
            {
                model.Message = Website.App_LocalResources.Error.PerformmingError;
            }

            return Json(new { result = model.Message });
        }
        public ActionResult CreateCategory()
        {
            return PartialView(new Models.EditCategoryViewModels
            {
                Status = false
            });
        }
        [HttpPost]
        public ActionResult CreateCategory(Models.EditCategoryViewModels model)
        {
            model.Message = string.Empty;
            if (category.Create(new Data.Category
            {
                Name = model.Name,
                Budget = model.Budget,
                Status = model.Status
            }, model.CampaignId) == false)
            {
                model.Message = Website.App_LocalResources.Error.PerformmingError;
            }

            return Json(new { result = model.Message });
        }
        #endregion
        #region Keyword
        // GET: ADV
        public ActionResult KeywordList(int page = 1, decimal URLId = 0)
        {
            IEnumerable<Models.KeywordListViewModels> result = from u in keyword.GetList(0, URLId, string.Empty, Service.ADV.Marching._None)
                                                               select new Models.KeywordListViewModels
                                                               {
                                                                   Id = u.KeywordId,
                                                                   Keyword = u.Keyword1,
                                                                   Budget = u.Budget,
                                                                   Status = u.Status,
                                                                   Bid = u.Bid,
                                                                   Marching = (Service.ADV.Marching)u.Marching
                                                               };

            int pageSize = 8;

            return PartialView(result.ToPagedList(page, pageSize));
        }
        public ActionResult DetailKeywordList(int page = 1, decimal keywordId = 0)
        {
            List<Models.DetailKeywordListViewModels> result = new List<Models.DetailKeywordListViewModels>();
            result.Add(new Models.DetailKeywordListViewModels
            {
                DisplayPageOrder = 1,
                SearchQuery = "<b>Dien thoai Sony</b> gia re",
                DisplayTime = DateTime.Now.ToString(),
                Cost = 2,
                Bid = 4,
                Status = true,
                SearchLocation = "Danang Vietnam"
            });

            int pageSize = 8;

            return PartialView(result.ToPagedList(page, pageSize));
        }
        public ActionResult KeywordDetail(decimal id)
        {
            Models.EditKeywordViewModels result = (from u in keyword.GetList(id, 0, string.Empty, Service.ADV.Marching._None)
                                                   select new Models.EditKeywordViewModels
                                                   {
                                                       Id = u.KeywordId,
                                                       Keyword = u.Keyword1,
                                                       Budget = u.Budget,
                                                       Status = u.Status,
                                                       Bid = u.Bid,
                                                       Marching = (Service.ADV.Marching)u.Marching
                                                   }).FirstOrDefault();

            ViewBag.NegativeMarchList = keyword.GetNegativeMarchs(id).ToList();
            ViewBag.Marching = (short)result.Marching;

            return PartialView(result);
        }
        public ActionResult DeleteKeyword(decimal id)
        {
            string message = string.Empty;
            if (!keyword.Dekete(id))
            {
                message = Website.App_LocalResources.Error.InuseKeyword;
            }

            return Json(new { result = message });
        }
        public ActionResult EditKeyword(Models.EditKeywordViewModels model)
        {
            model.Message = string.Empty;
            if (!keyword.Update(new Data.Keyword
            {
                KeywordId = model.Id,
                Keyword1 = model.Keyword,
                Budget = model.Budget,
                Status = model.Status,
                Bid = model.Bid,
                Marching = (short)model.Marching
            }, model.NegativeMarchs))
            {
                model.Message = Website.App_LocalResources.Error.DuplicatedKeyword;
            }

            return Json(new { result = model.Message });
        }
        public ActionResult CreateKeyword()
        {
            return PartialView(new Models.EditKeywordViewModels
            {
                Status = false
            });
        }
        [HttpPost]
        public ActionResult CreateKeyword(Models.EditKeywordViewModels model)
        {
            model.Message = string.Empty;
            if (keyword.Create(new Data.Keyword
            {
                KeywordId = model.Id,
                Keyword1 = model.Keyword,
                Budget = model.Budget,
                Status = model.Status,
                Bid = model.Bid,
                Marching = (short)model.Marching,
                URLId = model.URLId

            }, model.NegativeMarchs != null ? model.NegativeMarchs.ToList() : null) == false)
            {
                model.Message = Website.App_LocalResources.Error.DuplicatedKeyword;
            }

            return Json(new { result = model.Message });
        }
        #endregion
        #region URL
        // GET: ADV
        public ActionResult URLList(decimal categoryId, int page = 1)
        {
            IEnumerable<Models.URLListViewModels> result = from u in URL.GetList(0, string.Empty, categoryId)
                                                           select new Models.URLListViewModels
                                                           {
                                                               Id = u.Id,
                                                               URL = u.URL1,
                                                               Budget = u.Budget,
                                                               Status = u.Status,
                                                               DisplayTimes = 60,
                                                               CTR = 10,
                                                               AverageCPD = 2,
                                                               Cost = 90,
                                                               DisplayDevices = "PC 20% - Table 10% - Mobile 70%"
                                                           };

            int pageSize = 8;

            return PartialView(result.ToPagedList(page, pageSize));
        }
        public ActionResult KeywordDetailList(decimal keywordId, int page = 1)
        {
            List<Models.DetailKeywordListViewModels> result = new List<Models.DetailKeywordListViewModels>();
            result.Add(new Models.DetailKeywordListViewModels
            {
                DisplayPageOrder = 1,
                SearchQuery = "Dien thoai Sony gia re",
                DisplayTime = DateTime.Now.ToUniversalTime().ToString(),
                Bid = 4,
                Cost = 6,
                ClickAction = true,
                SearchLocation = "Danang Vietnam"
            });

            int pageSize = 8;

            return PartialView(result.ToPagedList(page, pageSize));
        }
        public ActionResult DeleteURL(decimal id)
        {
            string message = string.Empty;
            if (!URL.Dekete(id))
            {
                message = Website.App_LocalResources.Error.PerformmingError;
            }

            return Json(new { result = message });
        }
        public ActionResult EditURL(Models.EditURLViewModels model)
        {
            model.Message = string.Empty;
            if (!URL.Update(new Data.URL
            {
                Id = model.Id,
                URL1 = model.URL,
                Budget = model.Budget,
                Status = model.Status
            }))
            {
                model.Message = Website.App_LocalResources.Error.PerformmingError;
            }

            return Json(new { result = model.Message });
        }

        [HttpPost]
        public ActionResult UploadLinkFiles()
        {
            return UploadTempFiles("LINK");
        }
        [HttpPost]
        public ActionResult UploadFiles()
        {
            return UploadTempFiles();
        }
        JsonResult UploadTempFiles(string from = "URL")
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }

                        // Get the complete folder path and store the file inside it.  
                        fname = string.Format("{0}_{1}", User.Identity.Name.TrimStart(new char[] { '+' }), fname);
                        string tempFolder = "TempUploads";
                        if (from == "LINK")
                        {
                            tempFolder = "TempLinkUploads";
                        }
                        fname = Path.Combine(Server.MapPath(string.Format("~/{0}/", tempFolder)), fname);
                        //Clean
                        tempFolder = null;
                        file.SaveAs(fname);
                    }
                    // Returns message that successfully uploaded  
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }
        public ActionResult CreateURL()
        {
            return PartialView(new Models.EditURLViewModels
            {
                Status = false
            });
        }
        [HttpPost]
        public ActionResult CreateURL(Service.ADV.InputURLViewModels URLObj)
        {
            URL.ImageFolderPath = Server.MapPath("~/Uploads/");
            URL.TempImageFolderPath = Server.MapPath("~/TempUploads/");
            URL.LoginedUser = User.Identity.Name;

            string message = string.Empty;
            if (URL.Create(URLObj) == false)
            {
                //message = Website.App_LocalResources.Error.DuplicatedURL;
                message = URL.MessageCode;
            }
            else
            {
                //Add prices

            }

            return Json(new { result = message });
        }
        [HttpPost]
        public ActionResult EditURL(Service.ADV.InputURLViewModels URLObj)
        {
            URL.ImageFolderPath = Server.MapPath("~/Uploads/");
            URL.TempImageFolderPath = Server.MapPath("~/TempUploads/");
            URL.LoginedUser = User.Identity.Name;

            string message = string.Empty;
            if (URL.Update(URLObj) == false)
            {
                message = Website.App_LocalResources.Error.DuplicatedURL;
            }
            else
            {
                //Add prices

            }

            return Json(new { result = message });
        }
        public ActionResult URLDetail(decimal id, decimal categoryId)
        {

            Models.EditURLViewModels result = (from u in URL.GetList(id, string.Empty, categoryId)
                                               select new Models.EditURLViewModels
                                               {
                                                   Id = u.Id,
                                                   URL = u.URL1,
                                                   Budget = u.Budget,
                                                   Status = u.Status,
                                                   DisplayCurrencyId = u.DisplayCurrencyId,
                                                   DisplayLocationId = u.DisplayLocationId,
                                                   DisplayTimeframeId = u.DisplayTimeId,
                                                   DisplayTimes = u.DisplayTimes,
                                                   Devices = URL.GetURLDeviceList(0, string.Empty, u.Id).ToList(),
                                                   Prices = URL.GetURLPriceList(0, u.Id).ToList()
                                               }).FirstOrDefault();

            return PartialView(result);
        }
        #endregion
        public ActionResult CreateLink()
        {
            return View(new Models.DetailLinkViewModels());
        }

        //public ActionResult DeleteKeyword(int id)
        //{
        //    return View(new Models.DetailKeywordViewModels
        //    {
        //        Id = 1,
        //        Keyword = "Điện thoại",
        //        DisplayTypeId = 1,
        //        DisplayTypeName = "Đủ keywords (loại 1)",
        //        Exclude = "sony, apple, samsung",
        //        StatusId = 1,
        //        StatusName = "Hoạt động",
        //        Price = 4,
        //        DisplayNumber = 20,
        //        Rate = "PC: 20%; Tablet: 25%; Mobile: 55%",
        //        ClickedRate = 25,
        //        RealAVGPrice = 3,
        //        Total = 62
        //    });
        //}
        // GET: ADV/Edit/5
        //public ActionResult DetailLink(int id)
        //{
        //    List<Models.DetailKeywordViewModels> keywordList = new List<Models.DetailKeywordViewModels>();
        //    keywordList.Add(new Models.DetailKeywordViewModels
        //    {
        //        Id = 1,
        //        Keyword = "Điện thoại",
        //        DisplayTypeId = 1,
        //        DisplayTypeName = "Đủ keywords (loại 1)",
        //        Exclude = "sony, apple, samsung",
        //        StatusId = 1,
        //        StatusName = "Hoạt động",
        //        Price = 4,
        //        DisplayNumber = 20,
        //        Rate = "PC: 20%; Tablet: 25%; Mobile: 55%",
        //        ClickedRate = 25,
        //        RealAVGPrice = 3,
        //        Total = 62
        //    });

        //    ViewBag.KeywordList = keywordList;

        //    return View(new Models.DetailLinkViewModels
        //    {
        //        Id = id,
        //        URL = "http://link1",
        //        Picture = "http://link1.png",
        //        Title = "Link 1",
        //        Description = "Link 1",
        //        Budget = 2,
        //        Price = 2,
        //        UnitId = 1,
        //        DeviceName = "Desktop",
        //        UnitName = "USD",
        //        AreaName = "Da Nang - Viet Nam",
        //        TimeFrameName = "01 AM- 07 AM",
        //        AreaId = 1,
        //        TimeFrameId = 1,
        //        DisplayNumber = 1,
        //        CreatedDate = DateTime.Now
        //    });
        //}
        // POST: ADV/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ADV/Edit/5
        public ActionResult Edit(int id)
        {
            return View(new Models.EditCampaignViewModels
            {
                Id = 1,
                Name = "Summer Campaign",
                Budget = 2000
            });
        }
        //public ActionResult EditKeyword(decimal id)
        //{
        //    return View(new Models.DetailKeywordViewModels
        //    {
        //        Id = 1,
        //        Keyword = "Điện thoại",
        //        DisplayTypeId = 1,
        //        DisplayTypeName = "Đủ keywords (loại 1)",
        //        Exclude = "sony, apple, samsung",
        //        StatusId = 1,
        //        StatusName = "Hoạt động",
        //        Price = 4,
        //        DisplayNumber = 20,
        //        Rate = "PC: 20%; Tablet: 25%; Mobile: 55%",
        //        ClickedRate = 25,
        //        RealAVGPrice = 3,
        //        Total = 62
        //    });
        //}
        // GET: ADV/Edit/5
        public ActionResult EditLink(int id)
        {
            return View(new Models.DetailLinkViewModels
            {
                Id = id,
                URL = "http://link1",
                Picture = "http://link1.png",
                Title = "Link 1",
                Description = "Link 1",
                Budget = 2,
                Price = 2,
                UnitId = 1,
                AreaId = 1,
                TimeFrameId = 1,
                DisplayNumber = 1
            });
        }
        // POST: ADV/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ADV/Delete/5
        public ActionResult Delete(int id)
        {
            return View(new Models.EditCampaignViewModels
            {
                Id = 1,
                Name = "Summer Campaign",
                Budget = 2000
            });
        }
        // GET: ADV/Edit/5
        public ActionResult DeleteLink(int id)
        {
            return View(new Models.DetailLinkViewModels
            {
                Id = id,
                URL = "http://link1",
                Picture = "http://link1.png",
                Title = "Link 1",
                Description = "Link 1",
                Budget = 2,
                Price = 2,
                UnitId = 1,
                AreaId = 1,
                TimeFrameId = 1,
                DisplayNumber = 1,
                CreatedDate = DateTime.Now
            });
        }
        // POST: ADV/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
