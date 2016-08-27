using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //string html = System.IO.File.ReadAllText(Request.PhysicalPath + "/templates/register." + Request.Cookies[Lib.Common.Variables.CookieLanguageKey].Value + ".html");
            //HtmlDocument doc = new HtmlDocument();
            //doc.LoadHtml(html);
            //doc.GetElementbyId("")
            return View();
        }

        public ActionResult About()
        {
            string externalip = new System.Net.WebClient().DownloadString("http://ipinfo.io/ip");
            
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = string.Empty;

            return View();
        }
    }
}