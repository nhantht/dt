using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace DT.Controllers
{
    public class LanguageController : Controller
    {
        // GET: Language
        public ActionResult Index()
        {
            return View();
        }
        public void Change(string key)
        {
            if (string.IsNullOrEmpty(key) == false)
            {
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(key);
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(key);

                HttpCookie cookie = new HttpCookie(Lib.Common.Variables.CookieLanguageKey);
                cookie.Value = key;
                Response.Cookies.Add(cookie);
            }

            Response.Redirect(Request.UrlReferrer.ToString());
        }
    }
}