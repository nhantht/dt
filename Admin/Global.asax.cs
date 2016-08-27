using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Admin
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
            string key = "vi";

            if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
            {
                key = cookie.Value;
            }

            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(key);
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(key);

            //Clean
            cookie = null;
            key = null;
        }
    }
}
