using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Lib.Common
{
    public static class Application
    {
        public static string ApplicationPath()
        {
            return "http://" + HttpContext.Current.Request.Url.Authority;
        }
        public static string EscapePhone(string value)
        {
            return value.Replace("+", "");
        }
        public static string UnescapePhone(string value)
        {
            return string.Format("+{0}", value);
        }
    }
}
