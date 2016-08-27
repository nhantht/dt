using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class LanguageController : Controller
    {
        // GET: Language
        public ActionResult Index()
        {
            return View();
        }
        public void Change(string id)
        {
            if (string.IsNullOrEmpty(id) == false)
            {
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(id);
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(id);

                HttpCookie cookie = new HttpCookie(Lib.Common.Variables.CookieLanguageKey);
                cookie.Value = id;
                Response.Cookies.Add(cookie);
            }

            Response.Redirect(Request.UrlReferrer.ToString());
        }
    }
}