using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace Website
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_BeginRequest()
        {
            HttpCookie cookie = Request.Cookies[Lib.Common.Variables.CookieLanguageKey];
            string key = "en";

            if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
            {
                key = cookie.Value;
            }

            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(key);
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(key);

            HttpCookie session = Request.Cookies[Lib.Common.Variables.SessionKey];
            if (session != null)
            {
                Data.User user = new Service.User.Manage().GetUserInfoBySession(session.Value);
                //Log out
                if (user == null)
                {
                    Request.Cookies.Remove(Lib.Common.Variables.SessionKey);
                    FormsAuthentication.SignOut();
                }
                //Clean
                user = null;
            }
            //Clean
            session = null;
            cookie = null;
            key = null;
        }
    }
}
