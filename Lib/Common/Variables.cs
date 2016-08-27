using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lib.Common
{
    public static class Variables
    {
        public const string CookieLanguageKey = "DT.Language";
        public const string SessionKey = "DT.SessionKey";
        public const string Fullname = "DT.User.Fullname";
        public const string LoginCookie = "DT.User.LoginCookie";
        public const string AdminUser = "+84999999";
        public static string TempImageFolderPath = HttpContext.Current.Server.MapPath("~/Uploads/");
        public static string ImageFolderPath = HttpContext.Current.Server.MapPath("~/TempUploads/");

        public const string TempLinkImageFolder = "TempLinkUploads";
        public const string LinkImageFolder = "LinkUploads";
        public static string TempLinkImageFolderPath = HttpContext.Current.Server.MapPath("~/" + TempLinkImageFolder + "/");
        public static string LinkImageFolderPath = HttpContext.Current.Server.MapPath("~/" + LinkImageFolder + "/");
    }
}