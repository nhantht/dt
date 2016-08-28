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

namespace Website.Controllers
{
    public class IndexController : Controller
    {
        Service.Index.Link link = new Service.Index.Link();
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Detail(decimal id)
        {

            Models.IndexDetailModel result = (from u in link.GetList(id)
                                              select new Models.IndexDetailModel
                                              {
                                                  Id = u.Id,
                                                  Title = u.Title,
                                                  ShortDescription = u.ShortDescription,
                                                  Picture = u.Picture,
                                                  CreatedDate = u.CreatedDate,
                                                  URL = u.URL,
                                                  Price = u.Price
                                              }).FirstOrDefault();

            return PartialView(result);
        }
        public ActionResult List(int page = 1)
        {
            IEnumerable<Models.IndexDetailModel> result = from u in link.GetList(null)
                                                          orderby u.CreatedDate descending
                                                          where u.UserId == User.Identity.Name
                                                          select new Models.IndexDetailModel
                                                                {
                                                                    Id = u.Id,
                                                                    Title = u.Title,
                                                                    ShortDescription = u.ShortDescription,
                                                                    Picture = u.Picture,
                                                                    CreatedDate = u.CreatedDate,
                                                                    URL = u.URL,
                                                                    Price = u.Price
                                                                };

            int pageSize = 8;

            return PartialView(result.ToPagedList(page, pageSize));
        }
        [Authorize]
        public ActionResult Search()
        {
            return View();
        }
        Models.IndexDetailModel GetURLContent(string url, string priceRule)
        {
            string title = string.Empty;
            string description = string.Empty;
            string img = string.Empty;
            float price = 0;

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
                if (!string.IsNullOrEmpty(priceRule))
                {
                    try
                    {
                        price = float.Parse(GetMeta(string.Format("//{0}", priceRule), doc));
                    }
                    catch (Exception err)
                    {
                        price = 0;
                    }
                }
            }
            catch { }
            return new Models.IndexDetailModel { URL = url, Title = title, ShortDescription = description, Picture = img, Price = price };
        }
        List<string> GetChildURLs(string url)
        {
            List<string> urls = new List<string>();
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
                HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//a");
                foreach (HtmlNode node in nodes)
                {
                    urls.Add(node.Attributes[0].Value);
                }
            }
            catch { }
            return urls;
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
        [Authorize]
        public ActionResult Create()
        {
            return View(new Models.IndexDetailModel());
        }
        [HttpPost]
        [Authorize]
        public ActionResult Create(Models.IndexDetailModel model)
        {
            try
            {
                model.Message = string.Empty;
                if (link.Create(new Data.Link
                {
                    URL = Uri.UnescapeDataString(model.URL),
                    Picture = model.Picture == null ? string.Empty : Uri.UnescapeDataString(model.Picture),
                    Title = model.Title == null ? string.Empty : model.Title,
                    ShortDescription = model.Picture == null ? string.Empty : Uri.UnescapeDataString(model.ShortDescription),
                    Price = model.Price,
                    CreatedDate = DateTime.Now,
                    UserId = User.Identity.Name
                }, model.UnanalysedPicture) == false)
                {
                    if (model.IsOverride)
                    {
                        if (link.Update(new Data.Link
                        {
                            URL = Uri.UnescapeDataString(model.URL),
                            Picture = model.Picture == null ? string.Empty : Uri.UnescapeDataString(model.Picture),
                            Title = model.Title == null ? string.Empty : model.Title,
                            ShortDescription = model.Picture == null ? string.Empty : Uri.UnescapeDataString(model.ShortDescription),
                            Price = model.Price,
                            UserId = User.Identity.Name
                        }, model.UnanalysedPicture) == false)
                        {
                            model.Message = "Please contact an administrator!";
                        }
                    }
                    else
                    {
                        model.Message = "URL was added by another";
                    }
                }

                return Json(model);
            }
            catch (Exception err)
            {
                model.Message = err.Message;
                return Json(model);
            }
        }
        [HttpPost]
        [Authorize]
        public ActionResult Update(Models.IndexDetailModel model)
        {
            try
            {
                model.Message = string.Empty;
                if (link.Update(new Data.Link
                {
                    URL = Uri.UnescapeDataString(model.URL),
                    Picture = model.Picture == null ? string.Empty : Uri.UnescapeDataString(model.Picture),
                    Title = model.Title == null ? string.Empty : model.Title,
                    ShortDescription = model.ShortDescription == null ? string.Empty : Uri.UnescapeDataString(model.ShortDescription),
                    Price = model.Price,
                    UserId = User.Identity.Name
                }, model.UnanalysedPicture) == false)
                {
                    model.Message = "URL was added by another";
                }

                return Json(model);
            }
            catch (Exception err)
            {
                model.Message = err.Message;
                return Json(model);
            }
        }
        public ActionResult DetechURL()
        {
            return View(new Models.IndexDetailModel());
        }
        public ActionResult Analyse(Models.IndexDetailModel model)
        {
            List<Models.IndexDetailModel> result = new List<Models.IndexDetailModel>();
            if (string.IsNullOrEmpty(model.URL) == false)
            {
                if (model.AnalysedChildren)
                {
                    List<string> urls = GetChildURLs(model.URL);
                    foreach (string url in urls)
                    {
                        Models.IndexDetailModel temp = GetURLContent(url, model.PriceRule);
                        if (!string.IsNullOrEmpty(temp.Title))
                        {
                            result.Add(GetURLContent(url, model.PriceRule));
                        }
                    }
                }
                else
                {
                    result.Add(GetURLContent(model.URL, model.PriceRule));
                }
            }
            return View(result);
        }
    }
}
